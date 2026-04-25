using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class EconomicCalendarRiskEngine
    {
        private static readonly HashSet<string> HighImpactKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "CPI",
        "Inflation",
        "NFP",
        "Nonfarm Payrolls",
        "FOMC",
        "Interest Rate",
        "Rate Decision",
        "GDP",
        "Retail Sales",
        "Unemployment",
        "PMI",
        "PPI",
        "Central Bank",
        "Fed",
        "ECB",
        "BoE",
        "BoJ",
        "SARB"
    };

        public EconomicCalendarRiskResult Evaluate(
            ForexMarketInput input,
            DateTime nowUtc)
        {
            if (input.MacroEvents == null || input.MacroEvents.Count == 0)
                return EconomicCalendarRiskResult.Clear("No macro events supplied.");

            var pairCurrencies = GetCurrencies(input.Pair);

            var relevantEvents = input.MacroEvents
                .Where(x => pairCurrencies.Contains(x.Currency, StringComparer.OrdinalIgnoreCase))
                .Where(IsHighImpact)
                .Where(x => IsInsideRiskWindow(x.EventTimeUtc, nowUtc))
                .OrderBy(x => Math.Abs((x.EventTimeUtc - nowUtc).TotalMinutes))
                .ToList();

            if (relevantEvents.Count == 0)
                return EconomicCalendarRiskResult.Clear("No high-impact event risk detected.");

            var nearest = relevantEvents[0];

            return EconomicCalendarRiskResult.Block(
                $"High-impact event risk: {nearest.Currency} {nearest.Name} at {nearest.EventTimeUtc:u}.");
        }

        private static bool IsHighImpact(MacroEvent macroEvent)
        {
            if (macroEvent.Impact.Equals("HIGH", StringComparison.OrdinalIgnoreCase))
                return true;

            return HighImpactKeywords.Any(keyword =>
                macroEvent.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsInsideRiskWindow(DateTime eventTimeUtc, DateTime nowUtc)
        {
            var minutes = (eventTimeUtc - nowUtc).TotalMinutes;

            return minutes >= -30 && minutes <= 90;
        }

        private static HashSet<string> GetCurrencies(string pair)
        {
            var parts = pair.Split('/');

            if (parts.Length != 2)
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            return new HashSet<string>(parts, StringComparer.OrdinalIgnoreCase);
        }
    }
}
