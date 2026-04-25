using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class FxPricingEngine(CurrencyStrengthModel strength, FxRelativePricer pricer, FxPriceSimulator simulator)
    {
        public List<FxPrice> Run(List<MacroState> states, Dictionary<string, decimal> initialPrices)
        {
            if (states == null)
                throw new ArgumentNullException(nameof(states));

            if (initialPrices == null)
                throw new ArgumentNullException(nameof(initialPrices));

            if (states.Count == 0)
                return new List<FxPrice>();

            if (initialPrices.Count == 0)
                throw new ArgumentException("Initial prices are required.", nameof(initialPrices));

            return simulator.Simulate(
                states,
                initialPrices,
                strength,
                pricer);
        }

        public PricingResult Price(string signalPair, string signalDirection, decimal sizePositionSize)
        {
            if (string.IsNullOrWhiteSpace(signalPair))
                throw new ArgumentException("Signal pair is required.", nameof(signalPair));

            if (string.IsNullOrWhiteSpace(signalDirection))
                throw new ArgumentException("Signal direction is required.", nameof(signalDirection));

            if (sizePositionSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(sizePositionSize), "Position size must be greater than zero.");

            var direction = signalDirection.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(signalDirection));

            var pair = signalPair.Trim().ToUpperInvariant();

            var baseCurrency = pair.Length >= 6
                ? pair[..3]
                : string.Empty;

            var quoteCurrency = pair.Length >= 6
                ? pair[^3..]
                : string.Empty;

            if (string.IsNullOrWhiteSpace(baseCurrency) || string.IsNullOrWhiteSpace(quoteCurrency))
                throw new ArgumentException("Pair must be in a valid format, for example EURUSD or EUR/USD.", nameof(signalPair));

            var normalizedPair = $"{baseCurrency}/{quoteCurrency}";

            return new PricingResult
            {
                Pair = normalizedPair,
                Direction = direction,
                PositionSize = sizePositionSize,
                BaseCurrency = baseCurrency,
                QuoteCurrency = quoteCurrency,
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}