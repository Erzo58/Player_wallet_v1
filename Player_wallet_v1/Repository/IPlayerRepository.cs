namespace Player_wallet_v1.Repository
{
    public interface IPlayerRepository
    {
        Task<bool> CreatePlayerWalletAsync(Guid playerId);

        Task<decimal?> GetBalanceAsync(Guid playerId);

        Task UpdateBalanceAsync(Guid playerId, decimal newBalance);
    }
}
