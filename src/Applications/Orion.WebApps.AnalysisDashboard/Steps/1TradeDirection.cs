namespace Orion.WebApps.AnalysisDashboard.Steps
{
    /// <summary>
    /// Represents the direction of a trade or market bias
    /// </summary>
    public enum TradeDirectionEnum
    {
        /// <summary>
        /// No directional bias - neutral market stance
        /// </summary>
        Neutral = 0,

        /// <summary>
        /// Long position - expecting price to rise
        /// </summary>
        Long = 1,

        /// <summary>
        /// Short position - expecting price to fall
        /// </summary>
        Short = -1
    }

    /// <summary>
    /// Extension methods for TradeDirection
    /// </summary>
    public static class TradeDirectionExtensions
    {
        /// <summary>
        /// Returns the opposite direction
        /// </summary>
        public static TradeDirectionEnum Invert(this TradeDirectionEnum direction)
        {
            return direction switch
            {
                TradeDirectionEnum.Long => TradeDirectionEnum.Short,
                TradeDirectionEnum.Short => TradeDirectionEnum.Long,
                _ => TradeDirectionEnum.Neutral
            };
        }

        /// <summary>
        /// Returns a human-readable description of the direction
        /// </summary>
        public static string ToDisplayString(this TradeDirectionEnum direction)
        {
            return direction switch
            {
                TradeDirectionEnum.Long => "Long",
                TradeDirectionEnum.Short => "Short",
                TradeDirectionEnum.Neutral => "Neutral",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Returns an emoji representation of the direction
        /// </summary>
        public static string ToEmoji(this TradeDirectionEnum direction)
        {
            return direction switch
            {
                TradeDirectionEnum.Long => "📈",
                TradeDirectionEnum.Short => "📉",
                TradeDirectionEnum.Neutral => "➡️",
                _ => "❓"
            };
        }

        /// <summary>
        /// Returns true if the direction is directional (Long or Short)
        /// </summary>
        public static bool IsDirectional(this TradeDirectionEnum direction)
        {
            return direction != TradeDirectionEnum.Neutral;
        }

        /// <summary>
        /// Returns the sign multiplier for calculations (+1 for Long, -1 for Short, 0 for Neutral)
        /// </summary>
        public static int ToSign(this TradeDirectionEnum direction)
        {
            return (int)direction;
        }

        /// <summary>
        /// Converts a string to TradeDirection (case-insensitive)
        /// </summary>
        public static TradeDirectionEnum Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return TradeDirectionEnum.Neutral;

            return value.ToLowerInvariant() switch
            {
                "long" => TradeDirectionEnum.Long,
                "buy" => TradeDirectionEnum.Long,
                "bullish" => TradeDirectionEnum.Long,
                "1" => TradeDirectionEnum.Long,
                "short" => TradeDirectionEnum.Short,
                "sell" => TradeDirectionEnum.Short,
                "bearish" => TradeDirectionEnum.Short,
                "-1" => TradeDirectionEnum.Short,
                _ => TradeDirectionEnum.Neutral
            };
        }

        /// <summary>
        /// Determines if a price movement is favorable given the direction
        /// </summary>
        public static bool IsFavorableMovement(this TradeDirectionEnum direction, decimal entryPrice, decimal currentPrice)
        {
            return direction switch
            {
                TradeDirectionEnum.Long => currentPrice > entryPrice,
                TradeDirectionEnum.Short => currentPrice < entryPrice,
                _ => false
            };
        }

        /// <summary>
        /// Calculates the profit/loss percentage based on direction
        /// </summary>
        public static decimal CalculateProfitLossPercentage(this TradeDirectionEnum direction, decimal entryPrice, decimal currentPrice)
        {
            if (entryPrice <= 0)
                return 0;

            return direction switch
            {
                TradeDirectionEnum.Long => ((currentPrice - entryPrice) / entryPrice) * 100,
                TradeDirectionEnum.Short => ((entryPrice - currentPrice) / entryPrice) * 100,
                _ => 0
            };
        }
    }
}
