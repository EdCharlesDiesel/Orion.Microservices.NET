using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

/// <summary>
/// Contract for validating trade compliance before execution.
/// </summary>
public interface IComplianceEngine
{
    ComplianceResult Validate(
        string pair,
        string direction,
        decimal requestedSize,
        AccountSnapshot account,
        RealTimeRiskResult risk);
}