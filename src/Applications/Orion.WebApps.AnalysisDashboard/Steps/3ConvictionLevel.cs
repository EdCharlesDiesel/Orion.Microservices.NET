//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents the conviction level of a trading signal
//    /// Higher values indicate stronger confidence in the signal
//    /// </summary>
//    public enum ConvictionLevel
//    {
//        /// <summary>
//        /// No conviction - signal should be ignored
//        /// </summary>
//        None = 0,

//        /// <summary>
//        /// Low conviction - weak signal, consider with caution
//        /// </summary>
//        Low = 1,

//        /// <summary>
//        /// Medium conviction - moderate signal strength
//        /// </summary>
//        Medium = 2,

//        /// <summary>
//        /// High conviction - strong signal with multiple confirming factors
//        /// </summary>
//        High = 3,

//        /// <summary>
//        /// Very high conviction - exceptional signal with overwhelming evidence
//        /// </summary>
//        VeryHigh = 4
//    }

//    /// <summary>
//    /// Extension methods for ConvictionLevel
//    /// </summary>
//    public static class ConvictionLevelExtensions
//    {
//        /// <summary>
//        /// Returns a human-readable display string
//        /// </summary>
//        public static string ToDisplayString(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => "None",
//                ConvictionLevel.Low => "Low",
//                ConvictionLevel.Medium => "Medium",
//                ConvictionLevel.High => "High",
//                ConvictionLevel.VeryHigh => "Very High",
//                _ => "Unknown"
//            };
//        }

//        /// <summary>
//        /// Returns a color code associated with the conviction level
//        /// </summary>
//        public static string ToColorCode(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => "#808080",      // Gray
//                ConvictionLevel.Low => "#FFA500",        // Orange
//                ConvictionLevel.Medium => "#FFD700",     // Gold
//                ConvictionLevel.High => "#00FF00",       // Green
//                ConvictionLevel.VeryHigh => "#00BFFF",   // Deep Sky Blue
//                _ => "#808080"
//            };
//        }

//        /// <summary>
//        /// Returns a Bootstrap CSS class for the conviction level
//        /// </summary>
//        public static string ToBootstrapClass(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => "secondary",
//                ConvictionLevel.Low => "warning",
//                ConvictionLevel.Medium => "info",
//                ConvictionLevel.High => "success",
//                ConvictionLevel.VeryHigh => "primary",
//                _ => "secondary"
//            };
//        }

//        /// <summary>
//        /// Returns an emoji representing the conviction level
//        /// </summary>
//        public static string ToEmoji(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => "⚪",
//                ConvictionLevel.Low => "🟡",
//                ConvictionLevel.Medium => "🟠",
//                ConvictionLevel.High => "🟢",
//                ConvictionLevel.VeryHigh => "🔵",
//                _ => "⚪"
//            };
//        }

//        /// <summary>
//        /// Returns true if the conviction is sufficient for trading
//        /// </summary>
//        public static bool IsTradeable(this ConvictionLevel level)
//        {
//            return level >= ConvictionLevel.Medium;
//        }

//        /// <summary>
//        /// Returns true if the conviction is high enough for larger position sizes
//        /// </summary>
//        public static bool IsHighConfidence(this ConvictionLevel level)
//        {
//            return level >= ConvictionLevel.High;
//        }

//        /// <summary>
//        /// Returns the recommended position size multiplier (0.0 to 1.0)
//        /// </summary>
//        public static decimal GetPositionSizeMultiplier(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => 0m,
//                ConvictionLevel.Low => 0.25m,
//                ConvictionLevel.Medium => 0.5m,
//                ConvictionLevel.High => 0.75m,
//                ConvictionLevel.VeryHigh => 1.0m,
//                _ => 0m
//            };
//        }

//        /// <summary>
//        /// Returns the minimum strength score required for this conviction level
//        /// </summary>
//        public static int GetMinimumStrengthScore(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => 0,
//                ConvictionLevel.Low => 20,
//                ConvictionLevel.Medium => 40,
//                ConvictionLevel.High => 60,
//                ConvictionLevel.VeryHigh => 80,
//                _ => 0
//            };
//        }

//        /// <summary>
//        /// Returns the maximum strength score for this conviction level
//        /// </summary>
//        public static int GetMaximumStrengthScore(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => 19,
//                ConvictionLevel.Low => 39,
//                ConvictionLevel.Medium => 59,
//                ConvictionLevel.High => 79,
//                ConvictionLevel.VeryHigh => 100,
//                _ => 0
//            };
//        }

//        /// <summary>
//        /// Determines the conviction level from a strength score (0-100)
//        /// </summary>
//        public static ConvictionLevel FromStrengthScore(int score)
//        {
//            return score switch
//            {
//                < 20 => ConvictionLevel.None,
//                < 40 => ConvictionLevel.Low,
//                < 60 => ConvictionLevel.Medium,
//                < 80 => ConvictionLevel.High,
//                _ => ConvictionLevel.VeryHigh
//            };
//        }

//        /// <summary>
//        /// Determines the conviction level from a risk/reward ratio
//        /// </summary>
//        public static ConvictionLevel FromRiskRewardRatio(decimal ratio)
//        {
//            return ratio switch
//            {
//                < 1.0m => ConvictionLevel.None,
//                < 1.5m => ConvictionLevel.Low,
//                < 2.0m => ConvictionLevel.Medium,
//                < 3.0m => ConvictionLevel.High,
//                _ => ConvictionLevel.VeryHigh
//            };
//        }

//        /// <summary>
//        /// Returns true if this conviction level is higher than the specified one
//        /// </summary>
//        public static bool IsHigherThan(this ConvictionLevel current, ConvictionLevel other)
//        {
//            return current > other;
//        }

//        /// <summary>
//        /// Returns true if this conviction level is at least the specified minimum
//        /// </summary>
//        public static bool IsAtLeast(this ConvictionLevel current, ConvictionLevel minimum)
//        {
//            return current >= minimum;
//        }

//        /// <summary>
//        /// Combines multiple conviction levels to determine overall conviction
//        /// Uses weighted average approach
//        /// </summary>
//        public static ConvictionLevel Combine(params ConvictionLevel[] levels)
//        {
//            if (levels == null || levels.Length == 0)
//                return ConvictionLevel.None;

//            var totalWeight = 0;
//            var weightedSum = 0;

//            foreach (var level in levels)
//            {
//                var weight = level.GetAnalysisWeight();
//                weightedSum += (int)level * weight;
//                totalWeight += weight;
//            }

//            if (totalWeight == 0)
//                return ConvictionLevel.None;

//            var averageScore = (double)weightedSum / totalWeight;
//            var roundedLevel = (int)Math.Round(averageScore, MidpointRounding.AwayFromZero);

//            return (ConvictionLevel)Math.Clamp(roundedLevel, 0, 4);
//        }

//        /// <summary>
//        /// Returns the analysis weight for combining multiple conviction levels
//        /// </summary>
//        private static int GetAnalysisWeight(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => 0,
//                ConvictionLevel.Low => 1,
//                ConvictionLevel.Medium => 2,
//                ConvictionLevel.High => 3,
//                ConvictionLevel.VeryHigh => 4,
//                _ => 0
//            };
//        }

//        /// <summary>
//        /// Parses a string to ConvictionLevel
//        /// </summary>
//        public static ConvictionLevel Parse(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return ConvictionLevel.None;

//            var normalized = value.ToLowerInvariant().Trim();

//            return normalized switch
//            {
//                "none" or "0" => ConvictionLevel.None,
//                "low" or "1" => ConvictionLevel.Low,
//                "medium" or "med" or "2" => ConvictionLevel.Medium,
//                "high" or "3" => ConvictionLevel.High,
//                "veryhigh" or "very high" or "vhigh" or "4" => ConvictionLevel.VeryHigh,
//                _ => Enum.TryParse<ConvictionLevel>(value, true, out var result) ? result : ConvictionLevel.None
//            };
//        }

//        /// <summary>
//        /// Returns the typical risk per trade percentage for this conviction level
//        /// </summary>
//        public static decimal GetRecommendedRiskPercentage(this ConvictionLevel level)
//        {
//            return level switch
//            {
//                ConvictionLevel.None => 0m,
//                ConvictionLevel.Low => 0.5m,
//                ConvictionLevel.Medium => 1.0m,
//                ConvictionLevel.High => 1.5m,
//                ConvictionLevel.VeryHigh => 2.0m,
//                _ => 0m
//            };
//        }

//        /// <summary>
//        /// Returns a star rating representation (1-5 stars)
//        /// </summary>
//        public static string ToStarRating(this ConvictionLevel level)
//        {
//            var starCount = (int)level;
//            return new string('⭐', starCount) + new string('☆', 4 - starCount);
//        }

//        /// <summary>
//        /// Gets all conviction levels that are considered tradeable
//        /// </summary>
//        public static ConvictionLevel[] GetTradeableLevels()
//        {
//            return new[]
//            {
//                ConvictionLevel.Medium,
//                ConvictionLevel.High,
//                ConvictionLevel.VeryHigh
//            };
//        }

//    }
//}
