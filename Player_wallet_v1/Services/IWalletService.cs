using Player_wallet_v1.Model;
using Player_wallet_v1.Model.Dto;

namespace Player_wallet_v1.Services
{
    public interface IWalletService
    {
        Task<bool> RegisterPlayerWalletAsync(Guid playerId);
        Task<decimal> GetBalanceAsync(Guid playerId);
        Task<IEnumerable<TransactionRecordDto>> GetPlayerTransactionsAsync(Guid playerId);
        Task<TransactionResponseDto> ProcessTransactionAsync(Guid playerId, TransactionRequestDto request);
    }
}
