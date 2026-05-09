//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//        /// <summary>
//        /// Extension methods for TradingOpportunity to provide additional behavior
//        /// without modifying the core domain class
//        /// </summary>
//        public static class TradingSignalExtensions
//        {
//            /// <summary>
//            /// Determines if the signal is appropriate for a given risk profile
//            /// </summary>
//            public static bool IsSuitableForRiskProfile(this TradingOpportunity signal, RiskProfile profile)
//            {
//                if (signal == null) return false;
//                if (!signal.IsValid) return false;

//                return profile switch
//                {
//                    RiskProfile.Conservative =>
//                        signal.Conviction >= ConvictionLevel.High &&
//                        signal.RiskRewardRatio >= 3.0m &&
//                        signal.StrengthScore >= 70 &&
//                        signal.ConfirmingTimeframes >= 2,

//                    RiskProfile.Moderate =>
//                        signal.Conviction >= ConvictionLevel.Medium &&
//                        signal.RiskRewardRatio >= 2.0m &&
//                        signal.StrengthScore >= 50 &&
//                        signal.ConfirmingTimeframes >= 1,

//                    RiskProfile.Aggressive =>
//                        signal.Conviction >= ConvictionLevel.Low &&
//                        signal.RiskRewardRatio >= 1.5m &&
//                        signal.StrengthScore >= 30,

//                    _ => false
//                };
//            }

//            /// <summary>
//            /// Gets the signal strength as a descriptive category
//            /// </summary>
//            public static string GetStrengthCategory(this TradingOpportunity signal)
//            {
//                if (signal == null) return "Unknown";

//                return signal.StrengthScore switch
//                {
//                    >= 80 => "Very Strong",
//                    >= 60 => "Strong",
//                    >= 40 => "Moderate",
//                    >= 20 => "Weak",
//                    _ => "Very Weak"
//                };
//            }

//            /// <summary>
//            /// Returns a CSS class for UI styling based on signal strength
//            /// </summary>
//            public static string GetStrengthCssClass(this TradingOpportunity signal)
//            {
//                if (signal == null) return "strength-unknown";

//                return signal.StrengthScore switch
//                {
//                    >= 80 => "strength-very-strong",
//                    >= 60 => "strength-strong",
//                    >= 40 => "strength-moderate",
//                    >= 20 => "strength-weak",
//                    _ => "strength-very-weak"
//                };
//            }

//            /// <summary>
//            /// Returns a color hex code for the signal strength
//            /// </summary>
//            public static string GetStrengthColor(this TradingOpportunity signal)
//            {
//                if (signal == null) return "#808080";

//                return signal.StrengthScore switch
//                {
//                    >= 80 => "#00BFFF", // Deep Sky Blue - Very Strong
//                    >= 60 => "#00FF00", // Green - Strong
//                    >= 40 => "#FFD700", // Gold - Moderate
//                    >= 20 => "#FFA500", // Orange - Weak
//                    _ => "#FF4444"      // Red - Very Weak
//                };
//            }

//            /// <summary>
//            /// Returns an emoji representing the signal quality
//            /// </summary>
//            public static string GetQualityEmoji(this TradingOpportunity signal)
//            {
//                if (signal == null) return "❓";
//                if (!signal.IsValid) return "⚠️";

//                return signal.StrengthScore switch
//                {
//                    >= 80 => "🔥", // Fire - Excellent
//                    >= 60 => "💎", // Gem - Good
//                    >= 40 => "⭐", // Star - Decent
//                    >= 20 => "👀", // Eyes - Watch
//                    _ => "😴"      // Sleep - Poor
//                };
//            }

//            /// <summary>
//            /// Returns a Bootstrap badge class for the signal
//            /// </summary>
//            public static string GetBootstrapBadgeClass(this TradingOpportunity signal)
//            {
//                if (signal == null) return "bg-secondary";

//                var baseClass = signal.Direction == TradeDirection.Long ? "bg-success" : "bg-danger";

//                if (signal.Conviction >= ConvictionLevel.High)
//                    return $"{baseClass} fw-bold";

//                return baseClass;
//            }

//            /// <summary>
//            /// Gets a short summary of the signal suitable for lists
//            /// </summary>
//            public static string ToShortString(this TradingOpportunity signal)
//            {
//                if (signal == null) return "No signal";

//                var quality = signal.GetQualityEmoji();
//                var direction = signal.Direction == TradeDirection.Long ? "LONG" : "SHORT";

//                return $"{quality} {signal.Pair} {direction} | " +
//                       $"Entry: {signal.EntryPrice:F4} | " +
//                       $"R:R 1:{signal.RiskRewardRatio:F2} | " +
//                       $"Score: {signal.StrengthScore}/100";
//            }

//            /// <summary>
//            /// Returns a dictionary of key metrics for API responses
//            /// </summary>
//            public static Dictionary<string, object> ToApiResponse(this TradingOpportunity signal)
//            {
//                if (signal == null) return new Dictionary<string, object>();

//                return new Dictionary<string, object>
//                {
//                    ["id"] = signal.Id,
//                    ["pair"] = signal.Pair,
//                    ["direction"] = signal.Direction.ToString(),
//                    ["direction_emoji"] = signal.Direction.ToEmoji(),
//                    ["conviction"] = signal.Conviction.ToString(),
//                    ["conviction_emoji"] = signal.Conviction.ToEmoji(),
//                    ["strength_score"] = signal.StrengthScore,
//                    ["strength_category"] = signal.GetStrengthCategory(),
//                    ["quality_emoji"] = signal.GetQualityEmoji(),
//                    ["entry_price"] = signal.EntryPrice,
//                    ["stop_loss"] = signal.StopLossPrice,
//                    ["take_profit_1"] = signal.TakeProfit1Price,
//                    ["take_profit_2"] = signal.TakeProfit2Price,
//                    ["risk_reward_ratio"] = signal.RiskRewardRatio,
//                    ["risk_percentage"] = signal.RiskPercentage,
//                    ["profit_percentage"] = signal.PotentialProfitPercentage,
//                    ["stop_loss_pips"] = signal.StopLossPips,
//                    ["take_profit_pips"] = signal.TakeProfitPips,
//                    ["position_size"] = signal.RecommendedPositionSize,
//                    ["confirming_timeframes"] = signal.ConfirmingTimeframes,
//                    ["primary_timeframe"] = signal.PrimaryTimeframe.ToString(),
//                    ["is_valid"] = signal.IsValid,
//                    ["is_tradeable"] = signal.IsTradeable,
//                    ["is_high_conviction"] = signal.IsHighConviction,
//                    ["generated_at"] = signal.GeneratedAt.ToString("o"),
//                    ["expires_at"] = signal.ExpiresAt.ToString("o"),
//                    ["thesis"] = signal.Thesis,
//                    ["reasons_count"] = signal.Reasons.Count,
//                    ["timeframe_signals_count"] = signal.TimeframeSignals.Count
//                };
//            }

//            /// <summary>
//            /// Compares two signals to determine which is stronger
//            /// </summary>
//            public static int CompareStrength(this TradingOpportunity first, TradingOpportunity second)
//            {
//                if (first == null && second == null) return 0;
//                if (first == null) return -1;
//                if (second == null) return 1;

//                // Primary: Strength score
//                var scoreCompare = first.StrengthScore.CompareTo(second.StrengthScore);
//                if (scoreCompare != 0) return scoreCompare;

//                // Secondary: Risk/Reward ratio
//                var rrCompare = first.RiskRewardRatio.CompareTo(second.RiskRewardRatio);
//                if (rrCompare != 0) return rrCompare;

//                // Tertiary: Conviction level
//                return first.Conviction.CompareTo(second.Conviction);
//            }

//            /// <summary>
//            /// Determines if this signal is stronger than another
//            /// </summary>
//            public static bool IsStrongerThan(this TradingOpportunity signal, TradingOpportunity other)
//            {
//                return signal.CompareStrength(other) > 0;
//            }

//            /// <summary>
//            /// Checks if the signal meets minimum criteria for trading
//            /// </summary>
//            public static bool MeetsMinimumCriteria(this TradingOpportunity signal)
//            {
//                return signal != null &&
//                       signal.IsValid &&
//                       signal.StrengthScore >= 40 &&
//                       signal.RiskRewardRatio >= 1.5m &&
//                       signal.Conviction >= ConvictionLevel.Low &&
//                       signal.ConfirmingTimeframes >= 1;
//            }

//            /// <summary>
//            /// Checks if the signal is exceptional (very high quality)
//            /// </summary>
//            public static bool IsExceptional(this TradingOpportunity signal)
//            {
//                return signal != null &&
//                       signal.IsValid &&
//                       signal.StrengthScore >= 80 &&
//                       signal.RiskRewardRatio >= 3.0m &&
//                       signal.Conviction >= ConvictionLevel.High &&
//                       signal.ConfirmingTimeframes >= 2;
//            }

//            /// <summary>
//            /// Returns the remaining time until signal expiration
//            /// </summary>
//            public static TimeSpan GetTimeRemaining(this TradingOpportunity signal)
//            {
//                if (signal == null) return TimeSpan.Zero;
//                return signal.ExpiresAt - DateTime.UtcNow;
//            }

//            /// <summary>
//            /// Checks if the signal has expired
//            /// </summary>
//            public static bool HasExpired(this TradingOpportunity signal)
//            {
//                return signal != null && DateTime.UtcNow > signal.ExpiresAt;
//            }

//            /// <summary>
//            /// Checks if the signal is about to expire (within specified time)
//            /// </summary>
//            public static bool IsExpiringSoon(this TradingOpportunity signal, TimeSpan threshold)
//            {
//                if (signal == null || signal.HasExpired()) return false;
//                return signal.GetTimeRemaining() <= threshold;
//            }

//            /// <summary>
//            /// Returns a formatted expiration message
//            /// </summary>
//            public static string GetExpirationMessage(this TradingOpportunity signal)
//            {
//                if (signal == null) return string.Empty;

//                if (signal.HasExpired())
//                    return "⚠️ Expired";

//                var remaining = signal.GetTimeRemaining();

//                if (remaining.TotalHours < 1)
//                    return $"⏰ Expires in {remaining.Minutes} min";
//                if (remaining.TotalHours < 24)
//                    return $"⏰ Expires in {remaining.Hours} hrs";

//                return $"⏰ Expires in {remaining.Days} days";
//            }

//            /// <summary>
//            /// Validates the signal and returns a validation result
//            /// </summary>
//            public static SignalValidationResult Validate(this TradingOpportunity signal)
//            {
//                var result = new SignalValidationResult { IsValid = true };

//                if (signal == null)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Signal is null");
//                    return result;
//                }

//                // Basic validity
//                if (!signal.IsValid)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Signal fails basic validation");
//                }

//                // Direction validation
//                if (signal.Direction == TradeDirection.Neutral)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Signal has neutral direction");
//                }

//                // Price level validation
//                if (signal.EntryPrice <= 0)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Invalid entry price");
//                }

//                if (signal.StopLossPrice <= 0)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Invalid stop loss");
//                }

//                if (signal.TakeProfit1Price <= 0)
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Invalid take profit");
//                }

//                // Logical price validation
//                if (signal.Direction == TradeDirection.Long)
//                {
//                    if (signal.StopLossPrice >= signal.EntryPrice)
//                        result.Errors.Add("Stop loss above entry for long position");
//                    if (signal.TakeProfit1Price <= signal.EntryPrice)
//                        result.Errors.Add("Take profit below entry for long position");
//                }
//                else if (signal.Direction == TradeDirection.Short)
//                {
//                    if (signal.StopLossPrice <= signal.EntryPrice)
//                        result.Errors.Add("Stop loss below entry for short position");
//                    if (signal.TakeProfit1Price >= signal.EntryPrice)
//                        result.Errors.Add("Take profit above entry for short position");
//                }

//                // Warnings (don't invalidate, but worth noting)
//                if (signal.StrengthScore < 40)
//                    result.Warnings.Add("Low strength score");

//                if (signal.RiskRewardRatio < 1.5m)
//                    result.Warnings.Add("Poor risk/reward ratio");

//                if (signal.Conviction < ConvictionLevel.Medium)
//                    result.Warnings.Add("Low conviction");

//                if (signal.ConfirmingTimeframes == 0)
//                    result.Warnings.Add("No confirming timeframes");

//                if (!signal.Reasons.Any())
//                    result.Warnings.Add("No supporting reasons");

//                if (signal.HasExpired())
//                {
//                    result.IsValid = false;
//                    result.Errors.Add("Signal has expired");
//                }
//                else if (signal.IsExpiringSoon(TimeSpan.FromHours(1)))
//                {
//                    result.Warnings.Add("Signal expiring soon");
//                }

//                result.IsValid = result.IsValid && !result.Errors.Any();
//                return result;
//            }

//            /// <summary>
//            /// Gets a list of all issues (errors and warnings) with the signal
//            /// </summary>
//            public static List<string> GetAllIssues(this TradingOpportunity signal)
//            {
//                var validation = signal.Validate();
//                var issues = new List<string>();

//                issues.AddRange(validation.Errors.Select(e => $"❌ {e}"));
//                issues.AddRange(validation.Warnings.Select(w => $"⚠️ {w}"));

//                return issues;
//            }

//            /// <summary>
//            /// Creates a copy of the signal with updated expiration
//            /// </summary>
//            public static TradingOpportunity WithExtendedExpiration(this TradingOpportunity signal, TimeSpan extension)
//            {
//                if (signal == null) return null;

//                return TradingSignalBuilder.From(signal)
//                    .WithExpiration(DateTime.UtcNow.Add(extension))
//                    .Build();
//            }

//            /// <summary>
//            /// Calculates the potential profit in account currency
//            /// </summary>
//            public static decimal CalculateProfit(
//                this TradingOpportunity signal,
//                decimal positionSize,
//                decimal pipValuePerLot = 10m)
//            {
//                if (signal == null || positionSize <= 0) return 0;

//                var pips = signal.TakeProfitPips;
//                return pips * positionSize * pipValuePerLot;
//            }

//            /// <summary>
//            /// Calculates the potential loss in account currency
//            /// </summary>
//            public static decimal CalculateLoss(
//                this TradingOpportunity signal,
//                decimal positionSize,
//                decimal pipValuePerLot = 10m)
//            {
//                if (signal == null || positionSize <= 0) return 0;

//                var pips = signal.StopLossPips;
//                return pips * positionSize * pipValuePerLot;
//            }
//        }

//        /// <summary>
//        /// Result of signal validation
//        /// </summary>
//        public class SignalValidationResult
//        {
//            /// <summary>
//            /// Whether the signal is valid
//            /// </summary>
//            public bool IsValid { get; set; }

//            /// <summary>
//            /// Critical errors that invalidate the signal
//            /// </summary>
//            public List<string> Errors { get; set; } = new();

//            /// <summary>
//            /// Non-critical warnings
//            /// </summary>
//            public List<string> Warnings { get; set; } = new();

//            /// <summary>
//            /// Whether there are any warnings
//            /// </summary>
//            public bool HasWarnings => Warnings.Any();

//            /// <summary>
//            /// Whether there are any errors
//            /// </summary>
//            public bool HasErrors => Errors.Any();

//            /// <summary>
//            /// Returns a summary of the validation result
//            /// </summary>
//            public override string ToString()
//            {
//                if (IsValid && !HasWarnings)
//                    return "✅ Signal is valid";

//                if (IsValid && HasWarnings)
//                    return $"⚠️ Signal is valid with {Warnings.Count} warning(s)";

//                return $"❌ Signal is invalid with {Errors.Count} error(s)";
//            }

//            /// <summary>
//            /// Returns a detailed validation report
//            /// </summary>
//            public string GetDetailedReport()
//            {
//                var lines = new List<string> { ToString(), "" };

//                if (Errors.Any())
//                {
//                    lines.Add("Errors:");
//                    lines.AddRange(Errors.Select(e => $"  ❌ {e}"));
//                    lines.Add("");
//                }

//                if (Warnings.Any())
//                {
//                    lines.Add("Warnings:");
//                    lines.AddRange(Warnings.Select(w => $"  ⚠️ {w}"));
//                }

//                return string.Join(Environment.NewLine, lines);
//            }
//        }

//        /// <summary>
//        /// Extension methods for collections of TradingOpportunity
//        /// </summary>
//        public static class TradingSignalCollectionExtensions
//        {
//            /// <summary>
//            /// Filters signals suitable for a specific risk profile
//            /// </summary>
//            public static IEnumerable<TradingOpportunity> SuitableFor(
//                this IEnumerable<TradingOpportunity> signals,
//                RiskProfile profile)
//            {
//                if (signals == null) return Enumerable.Empty<TradingOpportunity>();

//                return signals
//                    .Where(s => s != null && s.IsSuitableForRiskProfile(profile))
//                    .OrderByDescending(s => s.StrengthScore);
//            }

//            /// <summary>
//            /// Gets the strongest signals from a collection
//            /// </summary>
//            public static IEnumerable<TradingOpportunity> GetStrongest(
//                this IEnumerable<TradingOpportunity> signals,
//                int count = 5)
//            {
//                if (signals == null) return Enumerable.Empty<TradingOpportunity>();

//                return signals
//                    .Where(s => s != null && s.IsValid)
//                    .OrderByDescending(s => s.StrengthScore)
//                    .ThenByDescending(s => s.RiskRewardRatio)
//                    .Take(count);
//            }

//            /// <summary>
//            /// Filters signals by minimum criteria
//            /// </summary>
//            public static IEnumerable<TradingOpportunity> MeetingMinimumCriteria(
//                this IEnumerable<TradingOpportunity> signals)
//            {
//                if (signals == null) return Enumerable.Empty<TradingOpportunity>();

//                return signals.Where(s => s != null && s.MeetsMinimumCriteria());
//            }

//            /// <summary>
//            /// Filters exceptional signals only
//            /// </summary>
//            public static IEnumerable<TradingOpportunity> ExceptionalOnly(
//                this IEnumerable<TradingOpportunity> signals)
//            {
//                if (signals == null) return Enumerable.Empty<TradingOpportunity>();

//                return signals.Where(s => s != null && s.IsExceptional());
//            }

//            /// <summary>
//            /// Groups signals by pair
//            /// </summary>
//            public static Dictionary<string, List<TradingOpportunity>> GroupByPair(
//                this IEnumerable<TradingOpportunity> signals)
//            {
//                if (signals == null) return new Dictionary<string, List<TradingOpportunity>>();

//                return signals
//                    .Where(s => s != null)
//                    .GroupBy(s => s.Pair)
//                    .ToDictionary(g => g.Key, g => g.ToList());
//            }

//            /// <summary>
//            /// Groups signals by conviction level
//            /// </summary>
//            public static Dictionary<ConvictionLevel, List<TradingOpportunity>> GroupByConviction(
//                this IEnumerable<TradingOpportunity> signals)
//            {
//                if (signals == null) return new Dictionary<ConvictionLevel, List<TradingOpportunity>>();

//                return signals
//                    .Where(s => s != null)
//                    .GroupBy(s => s.Conviction)
//                    .OrderByDescending(g => g.Key)
//                    .ToDictionary(g => g.Key, g => g.ToList());
//            }

//            /// <summary>
//            /// Calculates summary statistics for a collection of signals
//            /// </summary>
//            public static SignalCollectionStatistics CalculateStatistics(
//                this IEnumerable<TradingOpportunity> signals)
//            {
//                var stats = new SignalCollectionStatistics();
//                var validSignals = signals?.Where(s => s != null && s.IsValid).ToList()
//                    ?? new List<TradingOpportunity>();

//                if (!validSignals.Any()) return stats;

//                stats.TotalCount = validSignals.Count;
//                stats.LongCount = validSignals.Count(s => s.Direction == TradeDirection.Long);
//                stats.ShortCount = validSignals.Count(s => s.Direction == TradeDirection.Short);
//                stats.HighConvictionCount = validSignals.Count(s => s.IsHighConviction);
//                stats.ExceptionalCount = validSignals.Count(s => s.IsExceptional());
//                stats.AverageStrengthScore = validSignals.Average(s => s.StrengthScore);
//                stats.AverageRiskReward = validSignals.Average(s => s.RiskRewardRatio);
//                stats.BestSignal = validSignals.OrderByDescending(s => s.StrengthScore).FirstOrDefault();

//                return stats;
//            }
//        }

//        /// <summary>
//        /// Statistics for a collection of signals
//        /// </summary>
//        public class SignalCollectionStatistics
//        {
//            public int TotalCount { get; set; }
//            public int LongCount { get; set; }
//            public int ShortCount { get; set; }
//            public int HighConvictionCount { get; set; }
//            public int ExceptionalCount { get; set; }
//            public double AverageStrengthScore { get; set; }
//            public decimal AverageRiskReward { get; set; }
//            public TradingOpportunity BestSignal { get; set; }

//            public string GetSummary()
//            {
//                return $"Signals: {TotalCount} total | " +
//                       $"Long: {LongCount} | Short: {ShortCount} | " +
//                       $"High Conviction: {HighConvictionCount} | " +
//                       $"Exceptional: {ExceptionalCount} | " +
//                       $"Avg Score: {AverageStrengthScore:F1} | " +
//                       $"Avg R:R: 1:{AverageRiskReward:F2}";
//            }
//        }
//    }
