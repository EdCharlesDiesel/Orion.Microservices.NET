using Marten;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Repository.Interfaces;

namespace Orion.API.TradingEconomics.Repository
{
    public sealed class PostgresTradeRepository : ITradeRepository
    {
        private readonly IDocumentStore _store;

        public PostgresTradeRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task SaveTradeAsync(TradePlan trade)
        {
            await using var session = _store.LightweightSession();

            if (trade.Id == Guid.Empty)
                trade.Id = Guid.NewGuid();

            session.Store(trade);
            await session.SaveChangesAsync();
        }

        public async Task UpdateTradeAsync(TradePlan trade)
        {
            await using var session = _store.LightweightSession();

            session.Store(trade);
            await session.SaveChangesAsync();
        }

        public async Task<List<TradePlan>> GetOpenTradesAsync()
        {
            await using var session = _store.QuerySession();

            return (List<TradePlan>)await session
                .Query<TradePlan>()
                .Where(x => x.Status == "OPEN")
                .ToListAsync();
        }

        public async Task<List<TradePlan>> GetTodayTradesAsync()
        {
            await using var session = _store.QuerySession();

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return (List<TradePlan>)await session
                .Query<TradePlan>()
                .Where(x =>
                    (x.OpenedAt >= today && x.OpenedAt < tomorrow) ||
                    (x.ClosedAt.HasValue &&
                     x.ClosedAt.Value >= today &&
                     x.ClosedAt.Value < tomorrow))
                .ToListAsync();
        }

        public async Task<List<TradePlan>> GetClosedTradesAsync(DateTime fromUtc, DateTime toUtc)
        {
            await using var session = _store.QuerySession();

            return (List<TradePlan>)await session
                .Query<TradePlan>()
                .Where(x =>
                    x.Status == "CLOSED" &&
                    x.ClosedAt.HasValue &&
                    x.ClosedAt.Value >= fromUtc &&
                    x.ClosedAt.Value <= toUtc)
                .ToListAsync();
        }

        public async Task SaveOrderAsync(OrderRequest order)
        {
            await using var session = _store.LightweightSession();

            if (order.Id == Guid.Empty)
                order.Id = Guid.NewGuid();

            session.Store(order);
            await session.SaveChangesAsync();
        }

        public async Task SaveOrderStateAsync(OrderState state)
        {
            await using var session = _store.LightweightSession();

            if (state.Id == Guid.Empty)
                state.Id = Guid.NewGuid();

            session.Store(state);
            await session.SaveChangesAsync();
        }

        public async Task SaveAuditAsync(AuditRecord record)
        {
            await using var session = _store.LightweightSession();

            if (record.CorrelationId == Guid.Empty)
                record.CorrelationId = Guid.NewGuid();

            session.Store(record);
            await session.SaveChangesAsync();
        }
    }
}