using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

/// <summary>
/// Interface for economic calendar risk evaluation.
/// </summary>
public interface IEconomicCalendarRiskEngine
{
    /// <summary>
    /// Evaluates economic calendar risks for a given forex pair at a specific time.
    /// </summary>
    /// <param name="input">Market input containing pair and macro events.</param>
    /// <param name="nowUtc">Current UTC time for risk window calculation.</param>
    /// <returns>Risk evaluation result.</returns>
    EconomicCalendarRiskResult Evaluate(ForexMarketInput input, DateTime nowUtc);
}