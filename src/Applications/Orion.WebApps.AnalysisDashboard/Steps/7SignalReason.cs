//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents an individual reason or factor that contributes to a trading signal
//    /// Immutable value object that captures the rationale behind a trading decision
//    /// </summary>
//    public sealed record SignalReason
//    {
//        /// <summary>
//        /// Unique identifier for this reason
//        /// </summary>
//        public string Id { get; init; } = Guid.NewGuid().ToString("N");

//        /// <summary>
//        /// Human-readable description of the reason
//        /// Example: "RSI oversold on daily timeframe"
//        /// </summary>
//        public string Description { get; init; }

//        /// <summary>
//        /// Weight/importance of this reason (1-5)
//        /// Higher weight indicates stronger signal contribution
//        /// </summary>
//        public int Weight { get; init; }

//        /// <summary>
//        /// Category classification for this reason
//        /// </summary>
//        public SignalCategory Category { get; init; }

//        /// <summary>
//        /// When this reason was identified
//        /// </summary>
//        public DateTime Timestamp { get; init; }

//        /// <summary>
//        /// Optional: The timeframe this reason applies to
//        /// </summary>
//        public Timeframe? Timeframe { get; init; }

//        /// <summary>
//        /// Optional: The direction this reason suggests
//        /// </summary>
//        public TradeDirection? Direction { get; init; }

//        /// <summary>
//        /// Optional: Additional metadata about this reason
//        /// </summary>
//        public Dictionary<string, string> Metadata { get; init; } = new();

//        /// <summary>
//        /// Creates a new signal reason with default timestamp
//        /// </summary>
//        public SignalReason(string description, int weight, SignalCategory category)
//        {
//            if (string.IsNullOrWhiteSpace(description))
//                throw new ArgumentException("Description cannot be empty", nameof(description));

//            if (weight < 1 || weight > 5)
//                throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be between 1 and 5");

//            Description = description;
//            Weight = weight;
//            Category = category;
//            Timestamp = DateTime.UtcNow;
//        }

//        /// <summary>
//        /// Creates a signal reason with full parameters
//        /// </summary>
//        public SignalReason(
//            string description,
//            int weight,
//            SignalCategory category,
//            Timeframe? timeframe = null,
//            TradeDirection? direction = null,
//            Dictionary<string, string> metadata = null)
//            : this(description, weight, category)
//        {
//            Timeframe = timeframe;
//            Direction = direction;
//            Metadata = metadata ?? new Dictionary<string, string>();
//        }

//        /// <summary>
//        /// Returns the effective weight adjusted by category reliability
//        /// </summary>
//        public decimal GetEffectiveWeight()
//        {
//            var categoryReliability = Category.GetReliabilityScore() / 100m;
//            return Weight * categoryReliability;
//        }

//        /// <summary>
//        /// Returns a weighted score (positive for bullish, negative for bearish)
//        /// </summary>
//        public decimal GetDirectionalScore()
//        {
//            if (!Direction.HasValue || Direction.Value == TradeDirection.Neutral)
//                return 0;

//            var effectiveWeight = GetEffectiveWeight();
//            return Direction.Value == TradeDirection.Long ? effectiveWeight : -effectiveWeight;
//        }

//        /// <summary>
//        /// Returns true if this reason has high confidence
//        /// </summary>
//        public bool IsHighConfidence()
//        {
//            return Weight >= 4 && Category.GetReliabilityScore() >= 70;
//        }

//        /// <summary>
//        /// Returns a formatted string for display
//        /// </summary>
//        public string ToDisplayString()
//        {
//            var parts = new List<string>
//            {
//                $"{Category.ToEmoji()} {Description}",
//                $"[Weight: {Weight}/5]",
//                $"[{Category.ToDisplayString()}]"
//            };

//            if (Timeframe.HasValue)
//                parts.Add($"[{Timeframe.Value.ToDisplayString()}]");

//            if (Direction.HasValue)
//                parts.Add($"[{Direction.Value.ToEmoji()}]");

//            return string.Join(" ", parts);
//        }

//        /// <summary>
//        /// Returns a compact string for UI display
//        /// </summary>
//        public override string ToString()
//        {
//            var directionEmoji = Direction.HasValue ? Direction.Value.ToEmoji() : "";
//            var timeframeStr = Timeframe.HasValue ? $" ({Timeframe.Value.ToDisplayString()})" : "";

//            return $"{directionEmoji} {Description}{timeframeStr} [{Weight}/5]".Trim();
//        }
//    }

//    /// <summary>
//    /// Factory for creating common signal reasons
//    /// </summary>
//    public static class SignalReasonFactory
//    {
//        public static SignalReason CreateRsiOversold(decimal rsiValue, Timeframe timeframe)
//        {
//            return new SignalReason(
//                $"RSI oversold at {rsiValue:F1}",
//                weight: rsiValue < 20 ? 4 : 3,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: TradeDirection.Long,
//                metadata: new Dictionary<string, string> { ["RSI"] = rsiValue.ToString() }
//            );
//        }

//        public static SignalReason CreateRsiOverbought(decimal rsiValue, Timeframe timeframe)
//        {
//            return new SignalReason(
//                $"RSI overbought at {rsiValue:F1}",
//                weight: rsiValue > 80 ? 4 : 3,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: TradeDirection.Short,
//                metadata: new Dictionary<string, string> { ["RSI"] = rsiValue.ToString() }
//            );
//        }

//        public static SignalReason CreateMaCross(Timeframe timeframe, string fastMa, string slowMa, TradeDirection direction)
//        {
//            var directionText = direction == TradeDirection.Long ? "bullish" : "bearish";
//            return new SignalReason(
//                $"{fastMa}/{slowMa} {directionText} crossover",
//                weight: 3,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: direction,
//                metadata: new Dictionary<string, string>
//                {
//                    ["FastMA"] = fastMa,
//                    ["SlowMA"] = slowMa
//                }
//            );
//        }

//        public static SignalReason CreateTrendAlignment(int confirmingTimeframes, TradeDirection direction)
//        {
//            var directionText = direction == TradeDirection.Long ? "Bullish" : "Bearish";
//            var weight = confirmingTimeframes >= 3 ? 5 : (confirmingTimeframes >= 2 ? 3 : 2);

//            return new SignalReason(
//                $"{directionText} trend alignment across {confirmingTimeframes} timeframes",
//                weight: weight,
//                category: SignalCategory.Technical,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["ConfirmingTimeframes"] = confirmingTimeframes.ToString() }
//            );
//        }

//        public static SignalReason CreateSupportBounce(decimal price, decimal supportLevel, Timeframe timeframe)
//        {
//            var distance = ((price - supportLevel) / supportLevel) * 100;

//            return new SignalReason(
//                $"Bounce off support at {supportLevel:F4} ({distance:F2}% from level)",
//                weight: 4,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: TradeDirection.Long,
//                metadata: new Dictionary<string, string>
//                {
//                    ["Support"] = supportLevel.ToString(),
//                    ["CurrentPrice"] = price.ToString()
//                }
//            );
//        }

//        public static SignalReason CreateResistanceRejection(decimal price, decimal resistanceLevel, Timeframe timeframe)
//        {
//            var distance = ((resistanceLevel - price) / resistanceLevel) * 100;

//            return new SignalReason(
//                $"Rejection at resistance {resistanceLevel:F4} ({distance:F2}% from level)",
//                weight: 4,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: TradeDirection.Short,
//                metadata: new Dictionary<string, string>
//                {
//                    ["Resistance"] = resistanceLevel.ToString(),
//                    ["CurrentPrice"] = price.ToString()
//                }
//            );
//        }

//        public static SignalReason CreateDivergence(string indicator, TradeDirection direction, Timeframe timeframe)
//        {
//            var divergenceType = direction == TradeDirection.Long ? "bullish" : "bearish";

//            return new SignalReason(
//                $"{divergenceType} divergence on {indicator}",
//                weight: 4,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["Indicator"] = indicator }
//            );
//        }

//        public static SignalReason CreateVolumeSpike(decimal volumeRatio, TradeDirection direction, Timeframe timeframe)
//        {
//            var weight = volumeRatio >= 3 ? 5 : (volumeRatio >= 2 ? 4 : 3);

//            return new SignalReason(
//                $"Volume spike {volumeRatio:F1}x average",
//                weight: weight,
//                category: SignalCategory.Volume,
//                timeframe: timeframe,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["VolumeRatio"] = volumeRatio.ToString() }
//            );
//        }

//        public static SignalReason CreateMacroCatalyst(string eventName, TradeDirection direction, int weight = 4)
//        {
//            return new SignalReason(
//                eventName,
//                weight: weight,
//                category: SignalCategory.Macro,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["EventType"] = "Macro" }
//            );
//        }

//        public static SignalReason CreatePatternCompletion(string patternName, TradeDirection direction, Timeframe timeframe)
//        {
//            return new SignalReason(
//                $"{patternName} pattern completed",
//                weight: 4,
//                category: SignalCategory.Pattern,
//                timeframe: timeframe,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["Pattern"] = patternName }
//            );
//        }

//        public static SignalReason CreateFibonacciLevel(decimal level, TradeDirection direction, Timeframe timeframe)
//        {
//            var levelText = level == 0.382m ? "38.2%" :
//                           level == 0.5m ? "50%" :
//                           level == 0.618m ? "61.8%" :
//                           $"{level:P0}";

//            return new SignalReason(
//                $"Price reacting at Fibonacci {levelText} retracement",
//                weight: level == 0.618m ? 4 : 3,
//                category: SignalCategory.Technical,
//                timeframe: timeframe,
//                direction: direction,
//                metadata: new Dictionary<string, string> { ["FibLevel"] = level.ToString() }
//            );
//        }
//    }

//    /// <summary>
//    /// Collection of signal reasons with aggregation methods
//    /// </summary>
//    public class SignalReasonCollection
//    {
//        private readonly List<SignalReason> _reasons = new();

//        public IReadOnlyList<SignalReason> Reasons => _reasons.AsReadOnly();

//        public void Add(SignalReason reason) => _reasons.Add(reason);

//        public void AddRange(IEnumerable<SignalReason> reasons) => _reasons.AddRange(reasons);

//        /// <summary>
//        /// Calculates the total directional score (bullish positive, bearish negative)
//        /// </summary>
//        public decimal GetTotalDirectionalScore()
//        {
//            return _reasons.Sum(r => r.GetDirectionalScore());
//        }

//        /// <summary>
//        /// Returns the dominant direction based on weighted scores
//        /// </summary>
//        public TradeDirection GetDominantDirection()
//        {
//            var totalScore = GetTotalDirectionalScore();

//            if (totalScore > 1)
//                return TradeDirection.Long;
//            if (totalScore < -1)
//                return TradeDirection.Short;

//            return TradeDirection.Neutral;
//        }

//        /// <summary>
//        /// Calculates the overall conviction level based on reasons
//        /// </summary>
//        public ConvictionLevel CalculateConviction()
//        {
//            if (_reasons.Count == 0)
//                return ConvictionLevel.None;

//            var totalWeight = _reasons.Sum(r => r.GetEffectiveWeight());
//            var highConfidenceCount = _reasons.Count(r => r.IsHighConfidence());
//            var categoryDiversity = _reasons.Select(r => r.Category).Distinct().Count();

//            // Bonus for diverse signal categories
//            var diversityBonus = categoryDiversity >= 3 ? 1.5m : (categoryDiversity >= 2 ? 1.2m : 1.0m);
//            var adjustedScore = totalWeight * diversityBonus;

//            // Bonus for high confidence reasons
//            adjustedScore += highConfidenceCount * 2;

//            return adjustedScore switch
//            {
//                >= 20 => ConvictionLevel.VeryHigh,
//                >= 14 => ConvictionLevel.High,
//                >= 8 => ConvictionLevel.Medium,
//                >= 3 => ConvictionLevel.Low,
//                _ => ConvictionLevel.None
//            };
//        }

//        /// <summary>
//        /// Groups reasons by category
//        /// </summary>
//        public IReadOnlyDictionary<SignalCategory, List<SignalReason>> GroupByCategory()
//        {
//            return _reasons.GroupBy(r => r.Category)
//                          .ToDictionary(g => g.Key, g => g.ToList());
//        }

//        /// <summary>
//        /// Returns the most significant reason
//        /// </summary>
//        public SignalReason GetMostSignificantReason()
//        {
//            return _reasons.OrderByDescending(r => r.GetEffectiveWeight())
//                          .FirstOrDefault();
//        }

//        /// <summary>
//        /// Returns a summary of all reasons
//        /// </summary>
//        public string GetSummary()
//        {
//            if (_reasons.Count == 0)
//                return "No reasons provided";

//            var direction = GetDominantDirection();
//            var conviction = CalculateConviction();
//            var byCategory = GroupByCategory();

//            var summary = new List<string>
//            {
//                $"Direction: {direction.ToEmoji()} {direction.ToDisplayString()}",
//                $"Conviction: {conviction.ToEmoji()} {conviction.ToDisplayString()}",
//                $"Total Reasons: {_reasons.Count}",
//                "",
//                "By Category:"
//            };

//            foreach (var kvp in byCategory)
//            {
//                var totalWeight = kvp.Value.Sum(r => r.Weight);
//                summary.Add($"  {kvp.Key.ToEmoji()} {kvp.Key.ToDisplayString()}: {kvp.Value.Count} reasons (Weight: {totalWeight})");
//            }

//            return string.Join(Environment.NewLine, summary);
//        }
//    }
//}
