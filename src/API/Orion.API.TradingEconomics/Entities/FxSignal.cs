using System;

namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Represents a trading signal for a currency pair based on base and quote currency scores.
    /// </summary>
    public class FxSignal
    {
        private string? _pair;
        private const decimal DirectionThreshold = 0.0001m;

        /// <summary>
        /// Gets or sets the base currency (e.g., EUR in EUR/USD).
        /// </summary>
        public string BaseCurrency { get; set; } = default!;

        /// <summary>
        /// Gets or sets the quote currency (e.g., USD in EUR/USD).
        /// </summary>
        public string QuoteCurrency { get; set; } = default!;

        /// <summary>
        /// Gets the formatted currency pair (e.g., EUR/USD).
        /// </summary>
        public string Pair
        {
            get => _pair ?? $"{BaseCurrency}/{QuoteCurrency}";
            init => _pair = value;
        }

        /// <summary>
        /// Gets or sets the score for the base currency.
        /// </summary>
        public decimal BaseScore { get; set; }

        /// <summary>
        /// Gets or sets the score for the quote currency.
        /// </summary>
        public decimal QuoteScore { get; set; }

        /// <summary>
        /// Gets the difference between base and quote scores.
        /// </summary>
        public decimal SignalStrength => BaseScore - QuoteScore;

        /// <summary>
        /// Gets the trading direction (LONG or SHORT) based on signal strength.
        /// </summary>
        public string Direction => SignalStrength > DirectionThreshold ? "LONG" :
                                    SignalStrength < -DirectionThreshold ? "SHORT" :
                                    "NEUTRAL";

        /// <summary>
        /// Gets or sets the confidence level of the signal (0-1 range).
        /// </summary>
        public decimal Confidence { get; set; }

        /// <summary>
        /// Determines whether the signal is valid and actionable.
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(BaseCurrency) &&
                               !string.IsNullOrWhiteSpace(QuoteCurrency) &&
                               Confidence >= 0 &&
                               Confidence <= 1 &&
                               Direction != "NEUTRAL";

        /// <summary>
        /// Returns a string representation of the FxSignal.
        /// </summary>
        public override string ToString()
        {
            return $"{Pair} - {Direction} (Strength: {SignalStrength:F4}, Confidence: {Confidence:P0})";
        }

        /// <summary>
        /// Creates a deep copy of the signal.
        /// </summary>
        public FxSignal Clone()
        {
            return new FxSignal
            {
                BaseCurrency = this.BaseCurrency,
                QuoteCurrency = this.QuoteCurrency,
                _pair = this._pair,
                BaseScore = this.BaseScore,
                QuoteScore = this.QuoteScore,
                Confidence = this.Confidence
            };
        }
    }
}