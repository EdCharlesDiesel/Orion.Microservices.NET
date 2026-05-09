using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Provides FX pricing simulation and signal pricing metadata.
    /// </summary>
    public sealed class FxPricingEngine(CurrencyStrengthModel strength, FxRelativePricer pricer, FxPriceSimulator simulator) : IFxPricingEngine
    {
        private readonly CurrencyStrengthModel _strength = strength ?? throw new ArgumentNullException(nameof(strength));
        private readonly FxRelativePricer _pricer = pricer ?? throw new ArgumentNullException(nameof(pricer));
        private readonly FxPriceSimulator _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));

        /// <inheritdoc />
        public List<FxPrice> Run(
            List<MacroState> states,
            Dictionary<string, decimal> initialPrices)
        {
            ArgumentNullException.ThrowIfNull(states);
            ArgumentNullException.ThrowIfNull(initialPrices);

            if (states.Count == 0)
                return [];

            if (initialPrices.Count == 0)
                throw new ArgumentException("Initial prices are required.", nameof(initialPrices));

            return _simulator.Simulate(
                states,
                initialPrices,
                _strength,
                _pricer);
        }

        /// <inheritdoc />
        public PricingResult Price(
            string signalPair,
            string signalDirection,
            decimal positionSize)
        {
            if (string.IsNullOrWhiteSpace(signalPair))
                throw new ArgumentException("Signal pair is required.", nameof(signalPair));

            if (string.IsNullOrWhiteSpace(signalDirection))
                throw new ArgumentException("Signal direction is required.", nameof(signalDirection));

            if (positionSize <= 0m)
                throw new ArgumentOutOfRangeException(nameof(positionSize), "Position size must be greater than zero.");

            var direction = signalDirection.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(signalDirection));

            var clean = NormalizePair(signalPair);

            if (clean.Length != 6)
                throw new ArgumentException("Invalid pair format. Example: EURUSD or EUR/USD.", nameof(signalPair));

            var baseCcy = clean[..3];
            var quoteCcy = clean[3..];

            return new PricingResult
            {
                Pair = $"{baseCcy}/{quoteCcy}",
                Direction = direction,
                PositionSize = positionSize,
                BaseCurrency = baseCcy,
                QuoteCurrency = quoteCcy,
                TimestampUtc = DateTime.UtcNow
            };
        }

        private static string NormalizePair(string pair)
        {
            return pair
                .Trim()
                .ToUpperInvariant()
                .Replace("/", "")
                .Replace("-", "")
                .Replace("_", "");
        }
    }
}