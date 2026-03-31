using System.Collections.Concurrent;

namespace Player_wallet_v1.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ConcurrentDictionary<Guid, decimal> _wallet = new();

        public Task<bool> CreatePlayerWalletAsync(Guid playerId)
        {
            bool added = _wallet.TryAdd(playerId, 0m);
            return Task.FromResult(added);
        }

        public Task<decimal?> GetBalanceAsync(Guid playerId)
        {
            if (_wallet.TryGetValue(playerId, out var balance))
                return Task.FromResult<decimal?>(balance);

            return Task.FromResult<decimal?>(null);
        }

        public Task UpdateBalanceAsync(Guid playerId, decimal newBalance)
        {
            if (_wallet.ContainsKey(playerId))
            {
                _wallet[playerId] = newBalance;
            }
            return Task.CompletedTask;
        }
    }
}
