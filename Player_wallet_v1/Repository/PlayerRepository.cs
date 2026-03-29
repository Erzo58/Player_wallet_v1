using System.Collections.Concurrent;

namespace Player_wallet_v1.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ConcurrentDictionary<Guid, decimal> _wallets = new();

        public Task<bool> CreatePlayerWalletAsync(Guid playerId)
        {
            bool added = _wallets.TryAdd(playerId, 0m);
            return Task.FromResult(added);
        }

        public Task<decimal?> GetBalanceAsync(Guid playerId)
        {
            if (_wallets.TryGetValue(playerId, out var balance))
                return Task.FromResult<decimal?>(balance);

            return Task.FromResult<decimal?>(null);
        }

        public Task UpdateBalanceAsync(Guid playerId, decimal newBalance)
        {
            if (_wallets.ContainsKey(playerId))
            {
                _wallets[playerId] = newBalance;
            }
            return Task.CompletedTask;
        }
    }
}
