using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

/// <summary>
/// Interface for exit engine operations.
/// </summary>
public interface IExitEngine
{
    /// <summary>
    /// Determines whether a position should exit based on current candle.
    /// </summary>
    bool ShouldExit(OpenPosition position, Candle candle, out decimal exitPrice);
        
    /// <summary>
    /// Calculates exit plan including stop loss and take profit levels.
    /// </summary>
    ExitPlan Calculate(SignalResult signal, ExecutionOrder execution, RiskResult risk, List<NormalizedIndicator>? normalized);
}