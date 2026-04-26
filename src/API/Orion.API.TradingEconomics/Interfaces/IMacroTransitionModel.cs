using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Interfaces;

/// <summary>
/// Interface for macro transition model.
/// </summary>
public interface IMacroTransitionModel
{
    MacroState Next(MacroState current, ShockResult shocks, MarketRegime regime);
    MacroState NextWithNormalization(NormalizedIndicator normalized, ShockResult shocks, MarketRegime regime);
}