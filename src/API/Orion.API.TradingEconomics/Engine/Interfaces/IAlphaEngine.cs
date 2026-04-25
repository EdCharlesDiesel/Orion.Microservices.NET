using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

public interface IAlphaEngine
{
    AlphaResult Generate(string pair, List<NormalizedIndicator>? indicators, List<MacroEvent>? macroEvents = null);
    
}