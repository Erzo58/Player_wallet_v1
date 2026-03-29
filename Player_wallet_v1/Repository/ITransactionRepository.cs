using Player_wallet_v1.Model.Dto;

namespace Player_wallet_v1.Repository
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(TransactionRecordDto record);

        Task<IEnumerable<TransactionRecordDto>> GetByPlayerIdAsync(Guid playerId);

        Task<TransactionRecordDto?> GetByIdAsync(Guid transactionId);
    }
}
