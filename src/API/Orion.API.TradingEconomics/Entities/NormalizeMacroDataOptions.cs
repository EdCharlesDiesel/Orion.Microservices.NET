namespace Orion.API.TradingEconomics.Entities
{
    /// <summary>
    /// Configuration options for macro data normalization
    /// </summary>
    public class NormalizeMacroDataOptions
    {
        /// <summary>
        /// Remove duplicate candles with the same timestamp
        /// </summary>
        public bool RemoveDuplicates { get; set; } = true;

        /// <summary>
        /// Fill gaps in candle data using forward fill
        /// </summary>
        public bool FillGaps { get; set; } = false;

        /// <summary>
        /// Scale factor for prices (e.g., 0.1 for decimating forex pairs)
        /// </summary>
        public decimal PriceScaleFactor { get; set; } = 1.0m;

        /// <summary>
        /// Round prices to specified decimal places
        /// </summary>
        public int? RoundDecimals { get; set; } = null;

        /// <summary>
        /// Normalize timestamps to specific intervals (in minutes)
        /// </summary>
        public int? TimestampIntervalMinutes { get; set; } = null;

        /// <summary>
        /// Resample candles to target timeframe
        /// </summary>
        public TimeSpan? ResampleToTimeframe { get; set; } = null;

        /// <summary>
        /// Validate OHLC values (High >= Open/Close, Low <= Open/Close)
        /// </summary>
        public bool ValidateOHLC { get; set; } = true;
    }
}
