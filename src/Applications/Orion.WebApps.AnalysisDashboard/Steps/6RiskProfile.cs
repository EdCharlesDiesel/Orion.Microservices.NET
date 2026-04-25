//using Orion.WebApps.AanalysisDashboard.Models;

//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents the risk tolerance profile of a trader or trading strategy
//    /// Used to adjust position sizing, stop loss placement, and signal filtering
//    /// </summary>
//    public enum RiskProfile
//    {
//        /// <summary>
//        /// Capital preservation focus - smaller positions, wider stops, higher conviction required
//        /// </summary>
//        Conservative = 1,

//        /// <summary>
//        /// Balanced approach - moderate position sizes, standard risk parameters
//        /// </summary>
//        Moderate = 2,

//        /// <summary>
//        /// Growth focus - larger positions, tighter stops, accepts lower conviction signals
//        /// </summary>
//        Aggressive = 3
//    }

//    /// <summary>
//    /// Extension methods for RiskProfile
//    /// </summary>
//    public static class RiskProfileExtensions
//    {
//        /// <summary>
//        /// Returns a human-readable display string
//        /// </summary>
//        public static string ToDisplayString(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "Conservative",
//                RiskProfile.Moderate => "Moderate",
//                RiskProfile.Aggressive => "Aggressive",
//                _ => "Unknown"
//            };
//        }

//        /// <summary>
//        /// Returns an emoji representation
//        /// </summary>
//        public static string ToEmoji(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "🛡️",
//                RiskProfile.Moderate => "⚖️",
//                RiskProfile.Aggressive => "🎯",
//                _ => "❓"
//            };
//        }

//        /// <summary>
//        /// Returns the recommended risk per trade as percentage of account
//        /// </summary>
//        public static decimal GetRiskPerTrade(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 0.5m,    // 0.5% per trade
//                RiskProfile.Moderate => 1.0m,         // 1.0% per trade
//                RiskProfile.Aggressive => 2.0m,       // 2.0% per trade
//                _ => 1.0m
//            };
//        }

//        /// <summary>
//        /// Returns the maximum total portfolio risk as percentage
//        /// </summary>
//        public static decimal GetMaxPortfolioRisk(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 10m,      // Max 10% total exposure
//                RiskProfile.Moderate => 20m,          // Max 20% total exposure
//                RiskProfile.Aggressive => 35m,        // Max 35% total exposure
//                _ => 20m
//            };
//        }

//        /// <summary>
//        /// Returns the minimum conviction level required to take a trade
//        /// </summary>
//        public static ConvictionLevel GetMinimumConviction(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => ConvictionLevel.High,
//                RiskProfile.Moderate => ConvictionLevel.Medium,
//                RiskProfile.Aggressive => ConvictionLevel.Low,
//                _ => ConvictionLevel.Medium
//            };
//        }

//        /// <summary>
//        /// Returns the minimum risk/reward ratio required to take a trade
//        /// </summary>
//        public static decimal GetMinimumRiskRewardRatio(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 3.0m,     // At least 1:3 R:R
//                RiskProfile.Moderate => 2.0m,          // At least 1:2 R:R
//                RiskProfile.Aggressive => 1.5m,        // At least 1:1.5 R:R
//                _ => 2.0m
//            };
//        }

//        /// <summary>
//        /// Returns the position size multiplier based on risk profile
//        /// </summary>
//        public static decimal GetPositionSizeMultiplier(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 0.5m,      // Half size
//                RiskProfile.Moderate => 1.0m,          // Full size
//                RiskProfile.Aggressive => 1.5m,        // 1.5x size
//                _ => 1.0m
//            };
//        }

//        /// <summary>
//        /// Returns the ATR multiplier for stop loss placement
//        /// </summary>
//        public static decimal GetStopLossAtrMultiplier(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 2.0m,      // Wider stops (2x ATR)
//                RiskProfile.Moderate => 1.5m,          // Standard stops (1.5x ATR)
//                RiskProfile.Aggressive => 1.0m,        // Tighter stops (1x ATR)
//                _ => 1.5m
//            };
//        }

//        /// <summary>
//        /// Returns the maximum number of concurrent positions
//        /// </summary>
//        public static int GetMaxConcurrentPositions(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 3,
//                RiskProfile.Moderate => 5,
//                RiskProfile.Aggressive => 8,
//                _ => 5
//            };
//        }

//        /// <summary>
//        /// Returns the maximum drawdown allowed before stopping trading
//        /// </summary>
//        public static decimal GetMaxDrawdownPercentage(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 10m,       // Stop at 10% drawdown
//                RiskProfile.Moderate => 15m,           // Stop at 15% drawdown
//                RiskProfile.Aggressive => 25m,         // Stop at 25% drawdown
//                _ => 15m
//            };
//        }

//        /// <summary>
//        /// Returns true if this risk profile can trade during high volatility events
//        /// </summary>
//        public static bool AllowHighVolatilityTrading(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => false,
//                RiskProfile.Moderate => true,
//                RiskProfile.Aggressive => true,
//                _ => false
//            };
//        }

//        /// <summary>
//        /// Returns true if this risk profile can hold positions overnight
//        /// </summary>
//        public static bool AllowOvernightPositions(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => false,
//                RiskProfile.Moderate => true,
//                RiskProfile.Aggressive => true,
//                _ => true
//            };
//        }

//        /// <summary>
//        /// Returns true if this risk profile can hold positions over weekends
//        /// </summary>
//        public static bool AllowWeekendPositions(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => false,
//                RiskProfile.Moderate => false,
//                RiskProfile.Aggressive => true,
//                _ => false
//            };
//        }

//        /// <summary>
//        /// Returns the maximum position size as percentage of account
//        /// </summary>
//        public static decimal GetMaxPositionSizePercentage(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 5m,        // Max 5% in one position
//                RiskProfile.Moderate => 10m,           // Max 10% in one position
//                RiskProfile.Aggressive => 20m,         // Max 20% in one position
//                _ => 10m
//            };
//        }

//        /// <summary>
//        /// Returns the recommended leverage multiplier
//        /// </summary>
//        public static int GetRecommendedLeverage(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => 1,         // No leverage
//                RiskProfile.Moderate => 2,             // 2x leverage
//                RiskProfile.Aggressive => 5,           // 5x leverage
//                _ => 1
//            };
//        }

//        /// <summary>
//        /// Returns a CSS class for styling UI elements
//        /// </summary>
//        public static string ToCssClass(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "risk-conservative",
//                RiskProfile.Moderate => "risk-moderate",
//                RiskProfile.Aggressive => "risk-aggressive",
//                _ => "risk-default"
//            };
//        }

//        /// <summary>
//        /// Returns a Bootstrap badge class for UI display
//        /// </summary>
//        public static string ToBootstrapBadgeClass(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "bg-success",
//                RiskProfile.Moderate => "bg-warning",
//                RiskProfile.Aggressive => "bg-danger",
//                _ => "bg-secondary"
//            };
//        }

//        /// <summary>
//        /// Returns a color code for the risk profile
//        /// </summary>
//        public static string ToColorCode(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "#28a745", // Green
//                RiskProfile.Moderate => "#ffc107",      // Yellow
//                RiskProfile.Aggressive => "#dc3545",    // Red
//                _ => "#6c757d"                          // Gray
//            };
//        }

//        /// <summary>
//        /// Determines if a trading signal is suitable for this risk profile
//        /// </summary>
//        public static bool IsSignalSuitable(this RiskProfile profile, TradingSignal signal)
//        {
//            // Check conviction requirement
//            if (signal.Conviction < profile.GetMinimumConviction())
//                return false;

//            // Check risk/reward requirement
//            if (signal.RiskRewardRatio < profile.GetMinimumRiskRewardRatio())
//                return false;

//            // Additional conservative checks
//            if (profile == RiskProfile.Conservative)
//            {
//                // Conservatives need multiple timeframe confirmation
//                if (signal.ConfirmingTimeframes < 2)
//                    return false;

//                // Conservatives avoid high volatility
//                if (signal.AtrValue > signal.CurrentPrice * 0.02m) // >2% ATR
//                    return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Calculates the adjusted position size based on risk profile
//        /// </summary>
//        public static decimal CalculatePositionSize(
//            this RiskProfile profile,
//            decimal accountBalance,
//            decimal entryPrice,
//            decimal stopLossPrice,
//            decimal pipValue = 0.0001m)
//        {
//            var riskPerTrade = profile.GetRiskPerTrade();
//            var riskAmount = accountBalance * (riskPerTrade / 100);
//            var stopDistance = Math.Abs(entryPrice - stopLossPrice);

//            if (stopDistance == 0)
//                return 0;

//            var basePositionSize = riskAmount / stopDistance;
//            var adjustedSize = basePositionSize * profile.GetPositionSizeMultiplier();
//            var maxPositionValue = accountBalance * (profile.GetMaxPositionSizePercentage() / 100);
//            var maxPositionSize = maxPositionValue / entryPrice;

//            return Math.Min(adjustedSize, maxPositionSize);
//        }

//        /// <summary>
//        /// Parses a string to RiskProfile
//        /// </summary>
//        public static RiskProfile Parse(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return RiskProfile.Moderate;

//            var normalized = value.ToLowerInvariant().Trim();

//            return normalized switch
//            {
//                "conservative" or "cons" or "low" or "1" => RiskProfile.Conservative,
//                "moderate" or "mod" or "medium" or "med" or "2" => RiskProfile.Moderate,
//                "aggressive" or "agg" or "high" or "3" => RiskProfile.Aggressive,
//                _ => Enum.TryParse<RiskProfile>(value, true, out var result) ? result : RiskProfile.Moderate
//            };
//        }

//        /// <summary>
//        /// Returns a description of the risk profile
//        /// </summary>
//        public static string GetDescription(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative =>
//                    "Capital preservation focus. Requires high conviction signals with strong R:R ratios. " +
//                    "Uses smaller positions, wider stops, and avoids overnight/weekend exposure.",

//                RiskProfile.Moderate =>
//                    "Balanced approach to risk and reward. Accepts medium conviction signals with standard R:R ratios. " +
//                    "Uses moderate position sizes and can hold positions overnight.",

//                RiskProfile.Aggressive =>
//                    "Growth-oriented with higher risk tolerance. Accepts lower conviction signals. " +
//                    "Uses larger position sizes, tighter stops, and can hold through weekends and high volatility.",

//                _ => "Unknown risk profile"
//            };
//        }

//        /// <summary>
//        /// Returns the typical trader type associated with this profile
//        /// </summary>
//        public static string GetTraderType(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => "Position Trader / Investor",
//                RiskProfile.Moderate => "Swing Trader",
//                RiskProfile.Aggressive => "Day Trader / Scalper",
//                _ => "Unknown"
//            };
//        }

//        /// <summary>
//        /// Returns the recommended maximum holding period
//        /// </summary>
//        public static TimeSpan GetMaxHoldingPeriod(this RiskProfile profile)
//        {
//            return profile switch
//            {
//                RiskProfile.Conservative => TimeSpan.FromDays(30),
//                RiskProfile.Moderate => TimeSpan.FromDays(7),
//                RiskProfile.Aggressive => TimeSpan.FromHours(4),
//                _ => TimeSpan.FromDays(7)
//            };
//        }
//    }
//}
