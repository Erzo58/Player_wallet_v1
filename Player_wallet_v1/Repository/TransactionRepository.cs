using Player_wallet_v1.Model.Dto;
using System.Collections.Concurrent;

namespace Player_wallet_v1.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ConcurrentDictionary<Guid, TransactionRecordDto> _transactions = new();

        public Task AddTransactionAsync(TransactionRecordDto record)
        {
            _transactions.TryAdd(record.TransactionId, record);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TransactionRecordDto>> GetByPlayerIdAsync(Guid playerId)
        {
            var playerTxs = _transactions.Values
                .Where(t => t.PlayerId == playerId)
                .ToList();

            return Task.FromResult<IEnumerable<TransactionRecordDto>>(playerTxs);
        }

        public Task<TransactionRecordDto?> GetByIdAsync(Guid transactionId)
        {
            _transactions.TryGetValue(transactionId, out var tx);
            return Task.FromResult(tx);
        }
    }
}
