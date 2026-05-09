//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents trading signal analysis for a specific timeframe
//    /// Captures the technical and analytical state of a single timeframe
//    /// </summary>
//    public sealed record TimeframeSignal
//    {
//        /// <summary>
//        /// Unique identifier for this timeframe signal
//        /// </summary>
//        public string Id { get; init; } = Guid.NewGuid().ToString("N");

//        /// <summary>
//        /// The timeframe this analysis applies to
//        /// </summary>
//        public Timeframe Timeframe { get; init; }

//        /// <summary>
//        /// The directional bias for this timeframe
//        /// </summary>
//        public TradeDirection Direction { get; init; }

//        /// <summary>
//        /// Strength score for this timeframe's signal (0-100)
//        /// </summary>
//        public int StrengthScore { get; init; }

//        /// <summary>
//        /// Conviction level derived from the strength score
//        /// </summary>
//        public ConvictionLevel Conviction => ConvictionLevelExtensions.FromStrengthScore(StrengthScore);

//        /// <summary>
//        /// Current price at the time of analysis
//        /// </summary>
//        public decimal CurrentPrice { get; init; }

//        /// <summary>
//        /// RSI value for this timeframe (if calculated)
//        /// </summary>
//        public decimal? Rsi { get; init; }

//        /// <summary>
//        /// ATR value for this timeframe (if calculated)
//        /// </summary>
//        public decimal? Atr { get; init; }

//        /// <summary>
//        /// MACD value for this timeframe (if calculated)
//        /// </summary>
//        public decimal? Macd { get; init; }

//        /// <summary>
//        /// MACD signal line value (if calculated)
//        /// </summary>
//        public decimal? MacdSignal { get; init; }

//        /// <summary>
//        /// SMA 20 value (if calculated)
//        /// </summary>
//        public decimal? Sma20 { get; init; }

//        /// <summary>
//        /// SMA 50 value (if calculated)
//        /// </summary>
//        public decimal? Sma50 { get; init; }

//        /// <summary>
//        /// Bollinger Band upper value (if calculated)
//        /// </summary>
//        public decimal? BollingerUpper { get; init; }

//        /// <summary>
//        /// Bollinger Band lower value (if calculated)
//        /// </summary>
//        public decimal? BollingerLower { get; init; }

//        /// <summary>
//        /// Collection of reasons supporting this timeframe's signal
//        /// </summary>
//        public IReadOnlyList<SignalReason> Reasons { get; init; } = new List<SignalReason>();

//        /// <summary>
//        /// Textual analysis summary for this timeframe
//        /// </summary>
//        public string Analysis { get; init; } = string.Empty;

//        /// <summary>
//        /// Whether this timeframe shows a clear trend
//        /// </summary>
//        public bool HasClearTrend { get; init; }

//        /// <summary>
//        /// Whether this timeframe is in overbought/oversold territory
//        /// </summary>
//        public bool IsExtreme { get; init; }

//        /// <summary>
//        /// Volatility assessment for this timeframe
//        /// </summary>
//        public VolatilityLevel Volatility { get; init; }

//        /// <summary>
//        /// When this analysis was generated
//        /// </summary>
//        public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;

//        /// <summary>
//        /// Creates a new TimeframeSignal with validation
//        /// </summary>
//        public TimeframeSignal(
//            Timeframe timeframe,
//            TradeDirection direction,
//            int strengthScore,
//            decimal currentPrice)
//        {
//            if (strengthScore < 0 || strengthScore > 100)
//                throw new ArgumentOutOfRangeException(nameof(strengthScore), "Strength score must be between 0 and 100");

//            if (currentPrice <= 0)
//                throw new ArgumentException("Current price must be greater than zero", nameof(currentPrice));

//            Timeframe = timeframe;
//            Direction = direction;
//            StrengthScore = strengthScore;
//            CurrentPrice = currentPrice;
//        }

//        /// <summary>
//        /// Returns true if this timeframe's signal is tradeable
//        /// </summary>
//        public bool IsTradeable()
//        {
//            return Direction != TradeDirection.Neutral &&
//                   StrengthScore >= 40 &&
//                   HasClearTrend &&
//                   !IsExtreme;
//        }

//        /// <summary>
//        /// Returns true if this timeframe confirms another timeframe's direction
//        /// </summary>
//        public bool ConfirmsDirection(TradeDirection otherDirection)
//        {
//            return Direction == otherDirection && StrengthScore >= 50;
//        }

//        /// <summary>
//        /// Returns the weight multiplier for multi-timeframe analysis
//        /// </summary>
//        public decimal GetMultiplier()
//        {
//            var baseMultiplier = Timeframe.GetAnalysisWeight();

//            // Adjust based on signal strength
//            var strengthMultiplier = StrengthScore / 100m;

//            // Adjust based on clarity
//            var clarityMultiplier = HasClearTrend ? 1.2m : 0.8m;

//            // Reduce if extreme conditions
//            var extremeMultiplier = IsExtreme ? 0.7m : 1.0m;

//            return baseMultiplier * strengthMultiplier * clarityMultiplier * extremeMultiplier;
//        }

//        /// <summary>
//        /// Returns the trend status as a string
//        /// </summary>
//        public string GetTrendStatus()
//        {
//            if (!HasClearTrend)
//                return "Ranging/Consolidating";

//            return Direction switch
//            {
//                TradeDirection.Long => "Bullish Trend",
//                TradeDirection.Short => "Bearish Trend",
//                _ => "Neutral"
//            };
//        }

//        /// <summary>
//        /// Returns a formatted summary of this timeframe signal
//        /// </summary>
//        public string ToSummary()
//        {
//            var directionEmoji = Direction.ToEmoji();
//            var trendStatus = GetTrendStatus();
//            var rsiText = Rsi.HasValue ? $"RSI: {Rsi:F1}" : "";
//            var atrText = Atr.HasValue ? $"ATR: {Atr:F4}" : "";

//            return $"{Timeframe.ToEmoji()} {Timeframe.ToDisplayString()}: {directionEmoji} {Direction.ToDisplayString()} " +
//                   $"[{StrengthScore}/100] {Conviction.ToDisplayString()} | {trendStatus} | {rsiText} {atrText}".Trim();
//        }

//        /// <summary>
//        /// Returns a detailed analysis string
//        /// </summary>
//        public override string ToString()
//        {
//            var lines = new List<string>
//            {
//                ToSummary(),
//                $"Price: {CurrentPrice:F4}",
//                Analysis
//            };

//            if (Reasons.Any())
//            {
//                lines.Add("Supporting Factors:");
//                lines.AddRange(Reasons.Select(r => $"  • {r}"));
//            }

//            return string.Join(Environment.NewLine, lines);
//        }
//    }

//    /// <summary>
//    /// Volatility level enumeration for timeframe analysis
//    /// </summary>
//    public enum VolatilityLevel
//    {
//        VeryLow = 1,
//        Low = 2,
//        Normal = 3,
//        High = 4,
//        VeryHigh = 5
//    }

//    /// <summary>
//    /// Extension methods for VolatilityLevel
//    /// </summary>
//    public static class VolatilityLevelExtensions
//    {
//        public static string ToDisplayString(this VolatilityLevel level)
//        {
//            return level switch
//            {
//                VolatilityLevel.VeryLow => "Very Low",
//                VolatilityLevel.Low => "Low",
//                VolatilityLevel.Normal => "Normal",
//                VolatilityLevel.High => "High",
//                VolatilityLevel.VeryHigh => "Very High",
//                _ => "Unknown"
//            };
//        }

//        public static string ToEmoji(this VolatilityLevel level)
//        {
//            return level switch
//            {
//                VolatilityLevel.VeryLow => "😴",
//                VolatilityLevel.Low => "😊",
//                VolatilityLevel.Normal => "🙂",
//                VolatilityLevel.High => "😬",
//                VolatilityLevel.VeryHigh => "😱",
//                _ => "❓"
//            };
//        }

//        public static bool IsTradeable(this VolatilityLevel level)
//        {
//            return level >= VolatilityLevel.Low && level <= VolatilityLevel.High;
//        }
//    }

//    /// <summary>
//    /// Factory for creating TimeframeSignal instances
//    /// </summary>
//    public static class TimeframeSignalFactory
//    {
//        public static TimeframeSignal CreateFromMarketData(
//            Timeframe timeframe,
//            MarketDataFrame data,
//            TradeDirection direction,
//            List<SignalReason> reasons = null)
//        {
//            if (data == null || data.IsEmpty)
//                throw new ArgumentException("Market data cannot be empty", nameof(data));

//            var latest = data.Rows.Last();
//            var indicators = latest.Indicators;

//            // Calculate strength score based on indicators
//            var strengthScore = CalculateStrengthScore(data, direction);

//            // Determine if clear trend exists
//            var hasClearTrend = DetermineClearTrend(data);

//            // Determine if extreme conditions
//            var isExtreme = DetermineExtremeConditions(indicators);

//            // Calculate volatility level
//            var volatility = DetermineVolatilityLevel(indicators);

//            // Generate analysis text
//            var analysis = GenerateAnalysis(timeframe, direction, strengthScore, hasClearTrend, isExtreme, indicators);

//            return new TimeframeSignal(timeframe, direction, strengthScore, latest.Close)
//            {
//                Rsi = (decimal?)indicators.GetValueOrDefault("RSI"),
//                Atr = (decimal?)indicators.GetValueOrDefault("ATR"),
//                Macd = (decimal?)indicators.GetValueOrDefault("MACD"),
//                MacdSignal = (decimal?)indicators.GetValueOrDefault("MACD_Signal"),
//                Sma20 = (decimal?)indicators.GetValueOrDefault("SMA_20"),
//                Sma50 = (decimal?)indicators.GetValueOrDefault("SMA_50"),
//                BollingerUpper = (decimal?)indicators.GetValueOrDefault("BB_Upper"),
//                BollingerLower = (decimal?)indicators.GetValueOrDefault("BB_Lower"),
//                Reasons = reasons ?? new List<SignalReason>(),
//                HasClearTrend = hasClearTrend,
//                IsExtreme = isExtreme,
//                Volatility = volatility,
//                Analysis = analysis
//            };
//        }

//        private static int CalculateStrengthScore(MarketDataFrame data, TradeDirection direction)
//        {
//            var score = 50; // Base score

//            var latest = data.Rows.Last();
//            var indicators = latest.Indicators;

//            // RSI contribution
//            if (indicators.TryGetValue("RSI", out var rsi))
//            {
//                if (direction == TradeDirection.Long && rsi < 30) score += 15;
//                else if (direction == TradeDirection.Long && rsi < 40) score += 10;
//                else if (direction == TradeDirection.Short && rsi > 70) score += 15;
//                else if (direction == TradeDirection.Short && rsi > 60) score += 10;
//            }

//            // Trend contribution
//            var price = (double)latest.Close;
//            if (indicators.TryGetValue("SMA_20", out var sma20) &&
//                indicators.TryGetValue("SMA_50", out var sma50))
//            {
//                if (direction == TradeDirection.Long && price > sma20 && sma20 > sma50) score += 15;
//                else if (direction == TradeDirection.Short && price < sma20 && sma20 < sma50) score += 15;
//            }

//            // MACD contribution
//            if (indicators.TryGetValue("MACD", out var macd) &&
//                indicators.TryGetValue("MACD_Signal", out var macdSignal))
//            {
//                if (direction == TradeDirection.Long && macd > macdSignal) score += 10;
//                else if (direction == TradeDirection.Short && macd < macdSignal) score += 10;
//            }

//            return Math.Clamp(score, 0, 100);
//        }

//        private static bool DetermineClearTrend(MarketDataFrame data)
//        {
//            var latest = data.Rows.Last();
//            var indicators = latest.Indicators;

//            var price = (double)latest.Close;
//            if (indicators.TryGetValue("SMA_20", out var sma20))
//            {
//                var distanceFromSma = Math.Abs(price - sma20) / price;
//                return distanceFromSma > 0.01; // More than 1% from SMA
//            }

//            return false;
//        }

//        private static bool DetermineExtremeConditions(Dictionary<string, double> indicators)
//        {
//            if (indicators.TryGetValue("RSI", out var rsi))
//            {
//                return rsi < 20 || rsi > 80;
//            }
//            return false;
//        }

//        private static VolatilityLevel DetermineVolatilityLevel(Dictionary<string, decimal> indicators)
//        {
//            if (indicators.TryGetValue("ATR", out var atr) &&
//                indicators.TryGetValue("Close", out var price))
//            {
//                var atrPercentage = atr / price;

//                return atrPercentage switch
//                {
//                    < 0.005m => VolatilityLevel.VeryLow,
//                    < 0.01m => VolatilityLevel.Low,
//                    < 0.02m => VolatilityLevel.Normal,
//                    < 0.03m => VolatilityLevel.High,
//                    _ => VolatilityLevel.VeryHigh
//                };
//            }

//            return VolatilityLevel.Normal;
//        }

//        private static string GenerateAnalysis(
//            Timeframe timeframe,
//            TradeDirection direction,
//            int strengthScore,
//            bool hasClearTrend,
//            bool isExtreme,
//            Dictionary<string, decimal> indicators)
//        {
//            var parts = new List<string>();

//            // Direction and strength
//            var strengthDesc = strengthScore >= 70 ? "strong" :
//                              strengthScore >= 50 ? "moderate" : "weak";
//            parts.Add($"{timeframe.ToDisplayString()} shows {strengthDesc} {direction.ToDisplayString().ToLower()} bias");

//            // Trend status
//            if (hasClearTrend)
//                parts.Add("with clear trend structure");
//            else
//                parts.Add("in ranging/consolidating market");

//            // RSI info
//            if (indicators.TryGetValue("RSI", out var rsi))
//            {
//                var rsiCondition = rsi < 30 ? "oversold" :
//                                  rsi > 70 ? "overbought" : "neutral";
//                parts.Add($"(RSI: {rsi:F1} - {rsiCondition})");
//            }

//            // Extreme warning
//            if (isExtreme)
//                parts.Add("- EXTREME CONDITIONS DETECTED");

//            return string.Join(" ", parts);
//        }
//    }

//    /// <summary>
//    /// Collection for managing multiple timeframe signals
//    /// </summary>
//    public class MultiTimeframeAnalysis
//    {
//        private readonly Dictionary<Timeframe, TimeframeSignal> _signals = new();

//        public IReadOnlyDictionary<Timeframe, TimeframeSignal> Signals => _signals;

//        public void AddSignal(TimeframeSignal signal)
//        {
//            _signals[signal.Timeframe] = signal;
//        }

//        /// <summary>
//        /// Returns the primary (highest) timeframe signal
//        /// </summary>
//        public TimeframeSignal GetPrimarySignal()
//        {
//            return _signals.Values
//                .OrderByDescending(s => s.Timeframe)
//                .FirstOrDefault();
//        }

//        /// <summary>
//        /// Calculates overall direction from all timeframes
//        /// </summary>
//        public TradeDirection GetOverallDirection()
//        {
//            var weightedScore = _signals.Values
//                .Where(s => s.Direction != TradeDirection.Neutral)
//                .Sum(s => (int)s.Direction * s.GetMultiplier());

//            if (weightedScore > 0.5m) return TradeDirection.Long;
//            if (weightedScore < -0.5m) return TradeDirection.Short;
//            return TradeDirection.Neutral;
//        }

//        /// <summary>
//        /// Returns timeframes that confirm the overall direction
//        /// </summary>
//        public List<Timeframe> GetConfirmingTimeframes()
//        {
//            var overallDirection = GetOverallDirection();
//            return _signals.Values
//                .Where(s => s.ConfirmsDirection(overallDirection))
//                .Select(s => s.Timeframe)
//                .OrderByDescending(t => t)
//                .ToList();
//        }

//        /// <summary>
//        /// Returns true if all analyzed timeframes align
//        /// </summary>
//        public bool HasFullAlignment()
//        {
//            var directionalSignals = _signals.Values
//                .Where(s => s.Direction != TradeDirection.Neutral)
//                .ToList();

//            if (directionalSignals.Count < 2) return false;

//            var firstDirection = directionalSignals.First().Direction;
//            return directionalSignals.All(s => s.Direction == firstDirection);
//        }

//        /// <summary>
//        /// Generates a summary of all timeframe signals
//        /// </summary>
//        public string GetAnalysisSummary()
//        {
//            var lines = new List<string>
//            {
//                "=== MULTI-TIMEFRAME ANALYSIS ===",
//                $"Overall Direction: {GetOverallDirection().ToEmoji()} {GetOverallDirection().ToDisplayString()}",
//                $"Confirming Timeframes: {GetConfirmingTimeframes().Count}/{_signals.Count}",
//                $"Full Alignment: {(HasFullAlignment() ? "✅ Yes" : "❌ No")}",
//                ""
//            };

//            foreach (var signal in _signals.Values.OrderByDescending(s => s.Timeframe))
//            {
//                lines.Add(signal.ToSummary());
//            }

//            return string.Join(Environment.NewLine, lines);
//        }
//    }
//}
