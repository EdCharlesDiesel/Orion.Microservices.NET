using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Evaluates economic calendar risks for forex trading based on high-impact events.
    /// </summary>
    public sealed class EconomicCalendarRiskEngine : IEconomicCalendarRiskEngine
    {
        private static readonly HashSet<string> HighImpactKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "CPI", "Inflation", "NFP", "Nonfarm Payrolls", "FOMC",
            "Interest Rate", "Rate Decision", "GDP", "Retail Sales",
            "Unemployment", "PMI", "PPI", "Central Bank", "Fed",
            "ECB", "BoE", "BoJ", "SARB"
        };

        private const int PreEventWindowMinutes = 30;
        private const int PostEventWindowMinutes = 90;

        /// <inheritdoc />
        public EconomicCalendarRiskResult Evaluate(ForexMarketInput input, DateTime nowUtc)
        {
            ValidateInput(input);

            if (HasNoMacroEvents(input))
                return EconomicCalendarRiskResult.Clear("No macro events supplied.");

            var pairCurrencies = GetCurrenciesFromPair(input.Pair);
            var relevantEvents = FindRelevantEvents(input.MacroEvents, pairCurrencies, nowUtc);

            if (!relevantEvents.Any())
                return EconomicCalendarRiskResult.Clear("No high-impact event risk detected.");

            var nearestEvent = relevantEvents.First();
            return CreateRiskResult(nearestEvent);
        }

        private static void ValidateInput(ForexMarketInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrWhiteSpace(input.Pair))
                throw new ArgumentException("Pair is required.", nameof(input));
        }

        private static bool HasNoMacroEvents(ForexMarketInput input) =>
            input.MacroEvents == null || input.MacroEvents.Count == 0;

        private static HashSet<string> GetCurrenciesFromPair(string pair)
        {
            var parts = pair.Split('/');
            return parts.Length == 2
                ? new HashSet<string>(parts, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        private static List<MacroEvent> FindRelevantEvents(
            List<MacroEvent> events,
            HashSet<string> targetCurrencies,
            DateTime nowUtc)
        {
            return events
                .Where(x => targetCurrencies.Contains(x.Currency))
                .Where(IsHighImpact)
                .Where(x => IsInsideRiskWindow(x.EventTimeUtc, nowUtc))
                .OrderBy(x => GetMinutesUntilEvent(x.EventTimeUtc, nowUtc))
                .ToList();
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
            var minutes = GetMinutesUntilEvent(eventTimeUtc, nowUtc);
            return minutes >= -PreEventWindowMinutes && minutes <= PostEventWindowMinutes;
        }

        private static double GetMinutesUntilEvent(DateTime eventTimeUtc, DateTime nowUtc) =>
            (eventTimeUtc - nowUtc).TotalMinutes;

        private static EconomicCalendarRiskResult CreateRiskResult(MacroEvent nearestEvent) =>
            EconomicCalendarRiskResult.Block(
                $"High-impact event risk: {nearestEvent.Currency} {nearestEvent.Name} at {nearestEvent.EventTimeUtc:u}.");
    }
}