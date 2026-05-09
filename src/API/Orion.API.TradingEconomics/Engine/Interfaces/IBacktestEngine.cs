using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

/// <summary>
/// Contract for running historical backtests.
/// </summary>
public interface IBacktestEngine
{
    Task<List<TradeResult>> RunAsync(DateTime start, DateTime end, decimal capital);
}