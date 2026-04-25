using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Repository.Interfaces;

namespace Orion.API.TradingEconomics.Repository
{
    public sealed class InMemoryTradeRepository : ITradeRepository
    {
        private readonly List<TradePlan> _trades = new();
        private readonly List<OrderRequest> _orders = new();
        private readonly List<OrderState> _orderStates = new();
        private readonly List<AuditRecord> _audits = new();

        public Task SaveTradeAsync(TradePlan trade)
        {
            _trades.Add(trade);
            return Task.CompletedTask;
        }

        public Task UpdateTradeAsync(TradePlan trade)
        {
            var existing = _trades.FirstOrDefault(x =>
                x.Pair == trade.Pair &&
                x.OpenedAt == trade.OpenedAt);

            if (existing != null)
                _trades.Remove(existing);

            _trades.Add(trade);

            return Task.CompletedTask;
        }

        public Task<List<TradePlan>> GetOpenTradesAsync()
        {
            return Task.FromResult(
                _trades.Where(x => x.Status == "OPEN").ToList());
        }

        public Task<List<TradePlan>> GetTodayTradesAsync()
        {
            var today = DateTime.UtcNow.Date;

            return Task.FromResult(
                _trades
                    .Where(x => x.OpenedAt.Date == today || x.ClosedAt?.Date == today)
                    .ToList());
        }

        public Task<List<TradePlan>> GetClosedTradesAsync(DateTime fromUtc, DateTime toUtc)
        {
            return Task.FromResult(
                _trades
                    .Where(x =>
                        x.Status == "CLOSED" &&
                        x.ClosedAt >= fromUtc &&
                        x.ClosedAt <= toUtc)
                    .ToList());
        }

        public Task SaveOrderAsync(OrderRequest order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task SaveOrderStateAsync(OrderState state)
        {
            _orderStates.Add(state);
            return Task.CompletedTask;
        }

        public Task SaveAuditAsync(AuditRecord record)
        {
            _audits.Add(record);
            return Task.CompletedTask;
        }
    }
}
