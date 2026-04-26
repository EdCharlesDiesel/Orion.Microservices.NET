using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public class MacroTransitionModel : IMacroTransitionModel
    {
        public MacroState Next(MacroState current, ShockResult shocks, MarketRegime regime) => new()
        {
            GdpGrowth = current.GdpGrowth + shocks.GrowthShock,
            Inflation = current.Inflation + shocks.InflationShock,
            Sentiment = current.Sentiment + shocks.SentimentShock,
            IsStable = Math.Abs(shocks.GrowthShock) < 0.05m,
            TimestampUtc = DateTime.UtcNow
        };

        public MacroState NextWithNormalization(NormalizedIndicator normalized, ShockResult shocks, MarketRegime regime) => new()
        {
            GdpGrowth = normalized.GdpNormalized + shocks.GrowthShock,
            Inflation = normalized.InflationNormalized + shocks.InflationShock,
            Sentiment = normalized.SentimentNormalized + shocks.SentimentShock,
            IsStable = true,
            TimestampUtc = DateTime.UtcNow
        };
    }
}