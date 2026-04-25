using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Repository.Interfaces
{
    public interface ITradeRepository
    {
        Task SaveTradeAsync(TradePlan trade);
        Task UpdateTradeAsync(TradePlan trade);

        Task<List<TradePlan>> GetOpenTradesAsync();
        Task<List<TradePlan>> GetTodayTradesAsync();
        Task<List<TradePlan>> GetClosedTradesAsync(DateTime fromUtc, DateTime toUtc);

        Task SaveOrderAsync(OrderRequest order);
        Task SaveOrderStateAsync(OrderState state);

        Task SaveAuditAsync(AuditRecord record);
    }
}
