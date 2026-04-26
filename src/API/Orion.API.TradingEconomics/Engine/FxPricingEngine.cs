using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Provides FX pricing simulation and trade pricing metadata.
    /// </summary>
    public sealed class FxPricingEngine(
        CurrencyStrengthModel strength,
        FxRelativePricer pricer,
        FxPriceSimulator simulator) : IFxPricingEngine
    {
        /// <summary>
        /// Runs FX price simulation using macro states and initial prices.
        /// </summary>
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

            return simulator.Simulate(
                states,
                initialPrices,
                strength,
                pricer);
        }

        /// <summary>
        /// Builds a pricing result for a forex signal.
        /// </summary>
        public PricingResult Price(
            string signalPair,
            string signalDirection,
            decimal sizePositionSize)
        {
            if (string.IsNullOrWhiteSpace(signalPair))
                throw new ArgumentException("Signal pair is required.", nameof(signalPair));

            if (string.IsNullOrWhiteSpace(signalDirection))
                throw new ArgumentException("Signal direction is required.", nameof(signalDirection));

            if (sizePositionSize <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(sizePositionSize),
                    "Position size must be greater than zero.");

            var direction = signalDirection.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(signalDirection));

            var cleanPair = signalPair
                .Trim()
                .ToUpperInvariant()
                .Replace("/", string.Empty)
                .Replace("-", string.Empty)
                .Replace("_", string.Empty);

            if (cleanPair.Length != 6)
                throw new ArgumentException(
                    "Pair must be in a valid format, for example EURUSD or EUR/USD.",
                    nameof(signalPair));

            var baseCurrency = cleanPair[..3];
            var quoteCurrency = cleanPair[3..];
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