using Player_wallet_v1.Model;
using Player_wallet_v1.Model.Dto;
using System.Collections.Concurrent;


namespace Player_wallet_v1.Services
{
    public class WalletService : IWalletService
    {
        // In-memory data
        private readonly IPlayerRepository _playerRepo;
        private readonly ITransactionRepository _transactionRepo;

        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _playerLocks = new();

        public WalletService(IPlayerRepository playerRepo, ITransactionRepository transactionRepo)
        {
            _playerRepo = playerRepo;
            _transactionRepo = transactionRepo;
        }

        // 1.Register player
        public async Task<bool> RegisterPlayerWalletAsync(Guid playerId)
        {
            return await _playerRepo.CreatePlayerWalletAsync(playerId);
        }

        // 2. Get palyer balance
        public async Task<decimal?> GetBalanceAsync(Guid playerId)
        {
            return await _playerRepo.GetBalanceAsync(playerId);
        }

        // 3. Get transactions for a player
        public async Task<IEnumerable<TransactionDto>> GetPlayerTransactionsAsync(Guid playerId)
        {
            // player existence check
            var balance = await _playerRepo.GetBalanceAsync(playerId);
            if (balance == null) return null;

            var records = await _transactionRepo.GetByPlayerIdAsync(playerId);

            return records.Select(t => new TransactionDto(
                t.TransactionId,
                t.Type,
                t.Amount,
                DateTime.UtcNow
            ));
        }

        public async Task<TransactionResponseDto> ProcessTransactionAsync(Guid playerId, TransactionRequest request)
        {
            // get lock for player
            var playerLock = _playerLocks.GetOrAdd(playerId, _ => new SemaphoreSlim(1, 1));

            await playerLock.WaitAsync();
            try
            {
                // have thse transaction been processed before? (idempotency check)
                var existingTx = await _transactionRepo.GetByIdAsync(request.TransactionId);
                var currentBalance = await _playerRepo.GetBalanceAsync(playerId);

                if (currentBalance == null)
                    throw new Exception("Player not found.");

                if (existingTx != null)
                {
                    return new TransactionResponse(
                        existingTx.TransactionId,
                        existingTx.IsAccepted,
                        currentBalance.Value,
                        "Idempotent response: Already processed."
                    );
                }

                // B. Business Logika
                decimal newBalance = currentBalance.Value;
                if (request.Type == TransactionType.Stake)
                    newBalance -= request.Amount;
                else
                    newBalance += request.Amount;

                bool isAccepted = newBalance >= 0;

                // C. Zápis do repozitárov
                if (isAccepted)
                {
                    await _playerRepo.UpdateBalanceAsync(playerId, newBalance);
                }

                await _transactionRepo.AddTransactionAsync(new TransactionRecord
                {
                    TransactionId = request.TransactionId,
                    PlayerId = playerId,
                    Amount = request.Amount,
                    Type = request.Type,
                    IsAccepted = isAccepted
                });

                return new TransactionResponseDto(
                    request.TransactionId,
                    isAccepted,
                    isAccepted ? newBalance : currentBalance.Value,
                    isAccepted ? "Success" : "Insufficient funds"
                );
            }
            finally
            {
                playerLock.Release();
            }
        }
    }
}

