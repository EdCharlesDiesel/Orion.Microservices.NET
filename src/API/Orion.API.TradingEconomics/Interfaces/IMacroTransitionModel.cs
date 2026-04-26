using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces;

/// <summary>
/// Interface for macro transition model.
/// </summary>
public interface IMacroTransitionModel
{
    MacroState Next(MacroState current, ShockResult shocks, MarketRegime regime);
    MacroState NextWithNormalization(NormalizedIndicator normalized, ShockResult shocks, MarketRegime regime);
}