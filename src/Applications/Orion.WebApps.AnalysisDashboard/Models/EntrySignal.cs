//namespace Orion.WebApps.AanalysisDashboard.Models
//{
//    namespace Orion.Trading.Domain.Models
//    {
//        /// <summary>
//        /// Represents a specific entry trigger condition for trade execution
//        /// </summary>
//        public sealed class EntrySignal
//        {
//            private int _confidence;
//            private decimal _triggerPrice;
//            private decimal _price;

//            /// <summary>
//            /// Unique identifier for this entry signal
//            /// </summary>
//            public string Id { get; } = Guid.NewGuid().ToString("N");

//            /// <summary>
//            /// The trading pair this entry applies to
//            /// </summary>
//            public string Pair { get; init; } = string.Empty;

//            /// <summary>
//            /// The type of entry trigger
//            /// </summary>
//            public EntrySignalType Type { get; init; }

//            /// <summary>
//            /// The trading direction (-1: Short, 0: Neutral, 1: Long)
//            /// </summary>
//            public int Signal { get; set; }

//            /// <summary>
//            /// The trading direction as enum
//            /// </summary>
//            public TradeDirection Direction
//            {
//                get => Signal switch
//                {
//                    1 => TradeDirection.Long,
//                    -1 => TradeDirection.Short,
//                    _ => TradeDirection.Neutral
//                };
//                set => Signal = value switch
//                {
//                    TradeDirection.Long => 1,
//                    TradeDirection.Short => -1,
//                    _ => 0
//                };
//            }

//            /// <summary>
//            /// Confidence level (0-100)
//            /// </summary>
//            public int Confidence
//            {
//                get => _confidence;
//                set
//                {
//                    if (value < 0 || value > 100)
//                        throw new ArgumentOutOfRangeException(nameof(Confidence), "Confidence must be between 0 and 100");
//                    _confidence = value;
//                }
//            }

//            /// <summary>
//            /// Conviction level derived from confidence score
//            /// </summary>
//            public ConvictionLevel Conviction => Confidence switch
//            {
//                >= 80 => ConvictionLevel.VeryHigh,
//                >= 60 => ConvictionLevel.High,
//                >= 40 => ConvictionLevel.Medium,
//                >= 20 => ConvictionLevel.Low,
//                _ => ConvictionLevel.None
//            };

//            /// <summary>
//            /// List of reasons supporting this entry signal
//            /// </summary>
//            public List<string> Reasons { get; set; } = new();

//            /// <summary>
//            /// Current market price at signal generation
//            /// </summary>
//            public decimal Price
//            {
//                get => _price;
//                set
//                {
//                    if (value <= 0)
//                        throw new ArgumentException("Price must be greater than zero", nameof(Price));
//                    _price = value;
//                }
//            }

//            /// <summary>
//            /// The specific price level that triggers entry
//            /// </summary>
//            public decimal TriggerPrice
//            {
//                get => _triggerPrice;
//                init
//                {
//                    if (value <= 0)
//                        throw new ArgumentException("Trigger price must be greater than zero", nameof(TriggerPrice));
//                    _triggerPrice = value;
//                }
//            }

//            /// <summary>
//            /// Stochastic %K value (0-100)
//            /// </summary>
//            private decimal _stochK;
//            public decimal StochK
//            {
//                get => _stochK;
//                set
//                {
//                    if (value < 0 || value > 100)
//                        throw new ArgumentOutOfRangeException(nameof(StochK), "Stochastic %K must be between 0 and 100");
//                    _stochK = value;
//                }
//            }

//            /// <summary>
//            /// Stochastic %D value (0-100)
//            /// </summary>
//            private decimal _stochD;
//            public decimal StochD
//            {
//                get => _stochD;
//                set
//                {
//                    if (value < 0 || value > 100)
//                        throw new ArgumentOutOfRangeException(nameof(StochD), "Stochastic %D must be between 0 and 100");
//                    _stochD = value;
//                }
//            }

//            /// <summary>
//            /// RSI value (0-100)
//            /// </summary>
//            private decimal _rsi;
//            public decimal Rsi
//            {
//                get => _rsi;
//                set
//                {
//                    if (value < 0 || value > 100)
//                        throw new ArgumentOutOfRangeException(nameof(Rsi), "RSI must be between 0 and 100");
//                    _rsi = value;
//                }
//            }

//            /// <summary>
//            /// When this entry signal was generated
//            /// </summary>
//            public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;

//            /// <summary>
//            /// When this entry signal expires (if applicable)
//            /// </summary>
//            public DateTime? ExpiresAt { get; set; }

//            /// <summary>
//            /// The timeframe this entry signal is based on
//            /// </summary>
//            public Timeframe? Timeframe { get; set; }

//            /// <summary>
//            /// Distance from current price to trigger price as percentage
//            /// </summary>
//            public decimal DistancePercentage =>
//                Price > 0 ? Math.Abs((TriggerPrice - Price) / Price) * 100 : 0;

//            /// <summary>
//            /// Whether the trigger price is above current price
//            /// </summary>
//            public bool IsTriggerAbove => TriggerPrice > Price;

//            /// <summary>
//            /// Whether the trigger price is below current price
//            /// </summary>
//            public bool IsTriggerBelow => TriggerPrice < Price;

//            /// <summary>
//            /// Whether this is a valid entry signal
//            /// </summary>
//            public bool IsValid =>
//                Signal != 0 &&
//                Confidence >= 40 &&
//                Price > 0 &&
//                TriggerPrice > 0 &&
//                (!ExpiresAt.HasValue || DateTime.UtcNow < ExpiresAt.Value);

//            /// <summary>
//            /// Whether stochastic indicates oversold condition
//            /// </summary>
//            public bool IsStochasticOversold => StochK < 20 && StochD < 20;

//            /// <summary>
//            /// Whether stochastic indicates overbought condition
//            /// </summary>
//            public bool IsStochasticOverbought => StochK > 80 && StochD > 80;

//            /// <summary>
//            /// Whether RSI indicates oversold condition
//            /// </summary>
//            public bool IsRsiOversold => Rsi < 30;

//            /// <summary>
//            /// Whether RSI indicates overbought condition
//            /// </summary>
//            public bool IsRsiOverbought => Rsi > 70;

//            /// <summary>
//            /// Whether stochastic has a bullish crossover
//            /// </summary>
//            public bool HasBullishStochasticCrossover => StochK > StochD && StochK < 50;

//            /// <summary>
//            /// Whether stochastic has a bearish crossover
//            /// </summary>
//            public bool HasBearishStochasticCrossover => StochK < StochD && StochK > 50;

//            /// <summary>
//            /// Creates a new EntrySignal
//            /// </summary>
//            public EntrySignal()
//            {
//                GeneratedAt = DateTime.UtcNow;
//            }

//            /// <summary>
//            /// Creates a new EntrySignal with required parameters
//            /// </summary>
//            public EntrySignal(
//                string pair,
//                EntrySignalType type,
//                TradeDirection direction,
//                decimal price,
//                decimal triggerPrice,
//                int confidence = 50) : this()
//            {
//                Pair = !string.IsNullOrWhiteSpace(pair)
//                    ? pair
//                    : throw new ArgumentException("Pair cannot be empty", nameof(pair));

//                Type = type;
//                Direction = direction;
//                Price = price;
//                TriggerPrice = triggerPrice;
//                Confidence = confidence;
//            }

//            /// <summary>
//            /// Returns true if the entry condition is met at the given price
//            /// </summary>
//            public bool IsTriggered(decimal currentPrice)
//            {
//                if (!IsValid)
//                    return false;

//                return Direction switch
//                {
//                    TradeDirection.Long => IsTriggerAbove
//                        ? currentPrice >= TriggerPrice
//                        : currentPrice <= TriggerPrice,
//                    TradeDirection.Short => IsTriggerBelow
//                        ? currentPrice <= TriggerPrice
//                        : currentPrice >= TriggerPrice,
//                    _ => false
//                };
//            }

//            /// <summary>
//            /// Adds a reason to this entry signal
//            /// </summary>
//            public void AddReason(string reason)
//            {
//                if (!string.IsNullOrWhiteSpace(reason))
//                    Reasons.Add(reason);
//            }

//            /// <summary>
//            /// Adds multiple reasons to this entry signal
//            /// </summary>
//            public void AddReasons(params string[] reasons)
//            {
//                foreach (var reason in reasons)
//                    AddReason(reason);
//            }

//            /// <summary>
//            /// Sets stochastic values with validation
//            /// </summary>
//            public void SetStochastic(decimal k, decimal d)
//            {
//                StochK = k;
//                StochD = d;
//            }

//            /// <summary>
//            /// Returns a formatted display string
//            /// </summary>
//            public override string ToString()
//            {
//                var direction = Direction.ToEmoji();
//                var type = Type.ToEmoji();
//                var confidence = Conviction.ToEmoji();

//                return $"{type} {Type.ToDisplayString()} | {direction} {Direction.ToDisplayString()} " +
//                       $"@ {TriggerPrice:F4} | {confidence} {Confidence}% | RSI: {Rsi:F1}";
//            }

//            /// <summary>
//            /// Returns a detailed description of this entry signal
//            /// </summary>
//            public string ToDetailedString()
//            {
//                var lines = new List<string>
//            {
//                ToString(),
//                $"Pair: {Pair}",
//                $"Current Price: {Price:F4}",
//                $"Distance: {DistancePercentage:F2}% {(IsTriggerAbove ? "above" : "below")}",
//                $"Stochastic: K={StochK:F1} D={StochD:F1}",
//                Reasons.Any() ? "Reasons:" : null
//            };

//                foreach (var reason in Reasons)
//                    lines.Add($"  • {reason}");

//                return string.Join(Environment.NewLine, lines.Where(l => l != null));
//            }
//        }
//    }
//}
