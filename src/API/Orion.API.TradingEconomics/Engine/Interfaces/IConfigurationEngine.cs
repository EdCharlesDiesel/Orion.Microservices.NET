using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.Engine.Interfaces;

/// <summary>
/// Contract for accessing trading system configuration.
/// </summary>
public interface IConfigurationEngine
{
    TradingSystemConfig GetConfig();
    PairConfig GetPairConfig(string pair);
    bool IsLiveTradingEnabled();
    bool IsPairEnabled(string pair);
    RiskConfig GetRiskConfig();
    SignalConfig GetSignalConfig();
    ExecutionConfig GetExecutionConfig();
}