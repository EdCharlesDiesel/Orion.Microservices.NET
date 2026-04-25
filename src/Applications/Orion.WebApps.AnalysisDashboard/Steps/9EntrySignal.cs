//using ApexCharts;

//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents a specific entry trigger condition for trade execution
//    /// Defines the precise price level and conditions for entering a trade
//    /// </summary>
//    public sealed record EntrySignal
//    {
//        /// <summary>
//        /// Unique identifier for this entry signal
//        /// </summary>
//        public string Id { get; init; } = Guid.NewGuid().ToString("N");

//        /// <summary>
//        /// The type of entry trigger
//        /// </summary>
//        public EntrySignalType Type { get; init; }

//        /// <summary>
//        /// The specific price level that triggers entry
//        /// </summary>
//        public decimal TriggerPrice { get; init; }

//        /// <summary>
//        /// Current market price at time of signal generation
//        /// </summary>
//        public decimal CurrentPrice { get; init; }

//        /// <summary>
//        /// The trading pair this entry applies to
//        /// </summary>
//        public string Pair { get; init; } = string.Empty;

//        /// <summary>
//        /// The expected trade direction upon entry
//        /// </summary>
//        public TradeDirection Direction { get; init; }

//        /// <summary>
//        /// Human-readable description of the entry condition
//        /// </summary>
//        public string Description { get; init; } = string.Empty;

//        /// <summary>
//        /// Detailed rationale for this entry trigger
//        /// </summary>
//        public string Rationale { get; init; } = string.Empty;

//        /// <summary>
//        /// When this entry signal becomes valid
//        /// </summary>
//        public DateTime ValidFrom { get; init; }

//        /// <summary>
//        /// When this entry signal expires (if applicable)
//        /// </summary>
//        public DateTime? ValidUntil { get; init; }

//        /// <summary>
//        /// The timeframe this entry signal is based on
//        /// </summary>
//        public Timeframe? Timeframe { get; init; }

//        /// <summary>
//        /// Minimum volume required for entry confirmation
//        /// </summary>
//        public long? MinimumVolume { get; init; }

//        /// <summary>
//        /// Whether price must close beyond the trigger level
//        /// </summary>
//        public bool RequireCloseConfirmation { get; init; }

//        /// <summary>
//        /// Number of confirming candles required
//        /// </summary>
//        public int RequiredConfirmingCandles { get; init; } = 1;

//        /// <summary>
//        /// Maximum slippage allowed in pips
//        /// </summary>
//        public decimal MaxSlippagePips { get; init; }

//        /// <summary>
//        /// Whether this is an immediate entry (market order) or pending
//        /// </summary>
//        public bool IsImmediate { get; init; }

//        /// <summary>
//        /// The distance from current price to trigger price as percentage
//        /// </summary>
//        public decimal DistancePercentage =>
//            CurrentPrice > 0 ? Math.Abs((TriggerPrice - CurrentPrice) / CurrentPrice) * 100 : 0;

//        /// <summary>
//        /// The distance from current price to trigger price in pips
//        /// </summary>
//        public decimal DistanceInPips => Math.Abs(TriggerPrice - CurrentPrice) / 0.0001m;

//        /// <summary>
//        /// Whether the trigger price is above current price
//        /// </summary>
//        public bool IsTriggerAbove => TriggerPrice > CurrentPrice;

//        /// <summary>
//        /// Whether the trigger price is below current price
//        /// </summary>
//        public bool IsTriggerBelow => TriggerPrice < CurrentPrice;

//        /// <summary>
//        /// Additional metadata about this entry signal
//        /// </summary>
//        public Dictionary<string, string> Metadata { get; init; } = new();

//        /// <summary>
//        /// Creates a new EntrySignal with validation
//        /// </summary>
//        public EntrySignal(
//            EntrySignalType type,
//            decimal triggerPrice,
//            decimal currentPrice,
//            TradeDirection direction)
//        {
//            if (triggerPrice <= 0)
//                throw new ArgumentException("Trigger price must be greater than zero", nameof(triggerPrice));

//            if (currentPrice <= 0)
//                throw new ArgumentException("Current price must be greater than zero", nameof(currentPrice));

//            if (direction == TradeDirection.Neutral)
//                throw new ArgumentException("Direction must be Long or Short", nameof(direction));

//            Type = type;
//            TriggerPrice = triggerPrice;
//            CurrentPrice = currentPrice;
//            Direction = direction;
//            ValidFrom = DateTime.UtcNow;
//            MaxSlippagePips = type.GetSlippageTolerance();
//            RequireCloseConfirmation = type.IsConservative();
//        }

//        /// <summary>
//        /// Returns true if this entry signal is still valid
//        /// </summary>
//        public bool IsValid()
//        {
//            var now = DateTime.UtcNow;

//            if (now < ValidFrom)
//                return false;

//            if (ValidUntil.HasValue && now > ValidUntil.Value)
//                return false;

//            return true;
//        }

//        /// <summary>
//        /// Returns true if the entry condition is met at the given price
//        /// </summary>
//        public bool IsTriggered(decimal currentPrice, long? currentVolume = null)
//        {
//            if (!IsValid())
//                return false;

//            var priceConditionMet = Direction switch
//            {
//                TradeDirection.Long => IsTriggerAbove ? currentPrice >= TriggerPrice : currentPrice <= TriggerPrice,
//                TradeDirection.Short => IsTriggerBelow ? currentPrice <= TriggerPrice : currentPrice >= TriggerPrice,
//                _ => false
//            };

//            if (!priceConditionMet)
//                return false;

//            if (MinimumVolume.HasValue && currentVolume.HasValue)
//            {
//                return currentVolume.Value >= MinimumVolume.Value;
//            }

//            return true;
//        }

//        /// <summary>
//        /// Returns the recommended order type for this entry
//        /// </summary>
//        public OrderType GetRecommendedOrderType()
//        {
//            if (IsImmediate)
//                return OrderType.Market;

//            return Type.GetRecommendedOrderType();
//        }

//        /// <summary>
//        /// Returns the confirmation requirements as a string
//        /// </summary>
//        public string GetConfirmationRequirements()
//        {
//            var requirements = new List<string>();

//            if (RequireCloseConfirmation)
//                requirements.Add("Candle close confirmation required");

//            if (RequiredConfirmingCandles > 1)
//                requirements.Add($"{RequiredConfirmingCandles} confirming candles required");

//            if (MinimumVolume.HasValue)
//                requirements.Add($"Min volume: {MinimumVolume:N0}");

//            if (MaxSlippagePips > 0)
//                requirements.Add($"Max slippage: {MaxSlippagePips} pips");

//            return requirements.Any() ? string.Join(" | ", requirements) : "No special requirements";
//        }

//        /// <summary>
//        /// Returns a formatted display string
//        /// </summary>
//        public override string ToString()
//        {
//            var directionEmoji = Direction.ToEmoji();
//            var typeEmoji = Type.ToEmoji();
//            var triggerRelation = IsTriggerAbove ? "above" : "below";
//            var priceDiff = Math.Abs(TriggerPrice - CurrentPrice);
//            var priceDiffPercent = (priceDiff / CurrentPrice) * 100;

//            return $"{typeEmoji} {Type.ToDisplayString()} Entry | {directionEmoji} {Direction.ToDisplayString()} " +
//                   $"@ {TriggerPrice:F4} ({triggerRelation} current by {priceDiff:F4} / {priceDiffPercent:F2}%)";
//        }

//        /// <summary>
//        /// Returns a detailed description of this entry signal
//        /// </summary>
//        public string ToDetailedString()
//        {
//            var lines = new List<string>
//            {
//                ToString(),
//                $"Pair: {Pair}",
//                $"Current Price: {CurrentPrice:F4}",
//                $"Description: {Description}",
//                $"Rationale: {Rationale}",
//                $"Confirmation: {GetConfirmationRequirements()}",
//                $"Valid: {ValidFrom:yyyy-MM-dd HH:mm} UTC",
//                ValidUntil.HasValue ? $"Expires: {ValidUntil:yyyy-MM-dd HH:mm} UTC" : "No expiration",
//                Timeframe.HasValue ? $"Timeframe: {Timeframe.Value.ToDisplayString()}" : null
//            };

//            return string.Join(Environment.NewLine, lines.Where(l => l != null));
//        }

//        /// <summary>
//        /// Creates a copy of this entry signal with an updated trigger price
//        /// </summary>
//        public EntrySignal WithTriggerPrice(decimal newTriggerPrice)
//        {
//            return this with
//            {
//                TriggerPrice = newTriggerPrice,
//                Description = $"{Type.ToDisplayString()} at {newTriggerPrice:F4}"
//            };
//        }

//        /// <summary>
//        /// Creates a copy of this entry signal with an expiration time
//        /// </summary>
//        public EntrySignal WithExpiration(DateTime expiration)
//        {
//            return this with { ValidUntil = expiration };
//        }
//    }

//    /// <summary>
//    /// Factory for creating EntrySignal instances
//    /// </summary>
//    public static class EntrySignalFactory
//    {
//        /// <summary>
//        /// Creates a breakout entry signal
//        /// </summary>
//        public static EntrySignal CreateBreakout(
//            string pair,
//            decimal breakoutLevel,
//            decimal currentPrice,
//            TradeDirection direction,
//            Timeframe timeframe,
//            long? minimumVolume = null)
//        {
//            var triggerPrice = direction == TradeDirection.Long
//                ? breakoutLevel + (breakoutLevel * 0.001m)  // Add small buffer
//                : breakoutLevel - (breakoutLevel * 0.001m);

//            return new EntrySignal(EntrySignalType.Breakout, triggerPrice, currentPrice, direction)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                MinimumVolume = minimumVolume,
//                RequireCloseConfirmation = true,
//                RequiredConfirmingCandles = 1,
//                Description = $"Breakout {direction.ToDisplayString()} at {breakoutLevel:F4}",
//                Rationale = $"Price breaking {(direction == TradeDirection.Long ? "above resistance" : "below support")} " +
//                           $"at {breakoutLevel:F4} on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["BreakoutLevel"] = breakoutLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a pullback entry signal
//        /// </summary>
//        public static EntrySignal CreatePullback(
//            string pair,
//            decimal pullbackLevel,
//            decimal currentPrice,
//            TradeDirection direction,
//            Timeframe timeframe)
//        {
//            return new EntrySignal(EntrySignalType.Pullback, pullbackLevel, currentPrice, direction)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                RequireCloseConfirmation = false,
//                RequiredConfirmingCandles = 1,
//                IsImmediate = false,
//                Description = $"Pullback to {pullbackLevel:F4}",
//                Rationale = $"Entering on pullback to key {(direction == TradeDirection.Long ? "support" : "resistance")} " +
//                           $"level at {pullbackLevel:F4} within {direction.ToDisplayString()} trend",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["PullbackLevel"] = pullbackLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a support bounce entry signal
//        /// </summary>
//        public static EntrySignal CreateSupportBounce(
//            string pair,
//            decimal supportLevel,
//            decimal currentPrice,
//            Timeframe timeframe)
//        {
//            var triggerPrice = supportLevel + (supportLevel * 0.0005m); // Slight buffer above support

//            return new EntrySignal(EntrySignalType.SupportBounce, triggerPrice, currentPrice, TradeDirection.Long)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                RequireCloseConfirmation = true,
//                RequiredConfirmingCandles = 1,
//                Description = $"Support bounce at {supportLevel:F4}",
//                Rationale = $"Price bouncing off established support level at {supportLevel:F4} " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["SupportLevel"] = supportLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a resistance break entry signal
//        /// </summary>
//        public static EntrySignal CreateResistanceBreak(
//            string pair,
//            decimal resistanceLevel,
//            decimal currentPrice,
//            Timeframe timeframe,
//            long? minimumVolume = null)
//        {
//            var triggerPrice = resistanceLevel + (resistanceLevel * 0.001m);

//            return new EntrySignal(EntrySignalType.ResistanceBreak, triggerPrice, currentPrice, TradeDirection.Long)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                MinimumVolume = minimumVolume,
//                RequireCloseConfirmation = true,
//                RequiredConfirmingCandles = 2,
//                Description = $"Resistance break at {resistanceLevel:F4}",
//                Rationale = $"Price breaking above established resistance at {resistanceLevel:F4} " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["ResistanceLevel"] = resistanceLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a momentum entry signal
//        /// </summary>
//        public static EntrySignal CreateMomentum(
//            string pair,
//            decimal currentPrice,
//            TradeDirection direction,
//            Timeframe timeframe,
//            decimal momentumThreshold = 0.002m)
//        {
//            var triggerPrice = direction == TradeDirection.Long
//                ? currentPrice * (1 + momentumThreshold)
//                : currentPrice * (1 - momentumThreshold);

//            return new EntrySignal(EntrySignalType.Momentum, triggerPrice, currentPrice, direction)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                RequireCloseConfirmation = false,
//                IsImmediate = true,
//                Description = $"Momentum {direction.ToDisplayString()} entry",
//                Rationale = $"Strong {direction.ToDisplayString().ToLower()} momentum detected " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["MomentumThreshold"] = momentumThreshold.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a Fibonacci level entry signal
//        /// </summary>
//        public static EntrySignal CreateFibonacciEntry(
//            string pair,
//            decimal fibLevel,
//            decimal currentPrice,
//            TradeDirection direction,
//            Timeframe timeframe,
//            decimal fibRatio)
//        {
//            return new EntrySignal(EntrySignalType.FibonacciLevel, fibLevel, currentPrice, direction)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                RequireCloseConfirmation = true,
//                RequiredConfirmingCandles = 1,
//                Description = $"Fibonacci {fibRatio:P0} entry at {fibLevel:F4}",
//                Rationale = $"Price reacting at Fibonacci {fibRatio:P0} retracement level " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["FibRatio"] = fibRatio.ToString(),
//                    ["FibLevel"] = fibLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates a retest entry signal
//        /// </summary>
//        public static EntrySignal CreateRetest(
//            string pair,
//            decimal retestLevel,
//            decimal currentPrice,
//            TradeDirection direction,
//            Timeframe timeframe)
//        {
//            return new EntrySignal(EntrySignalType.Retest, retestLevel, currentPrice, direction)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                RequireCloseConfirmation = true,
//                RequiredConfirmingCandles = 1,
//                Description = $"Retest of {retestLevel:F4}",
//                Rationale = $"Price retesting previously broken {(direction == TradeDirection.Long ? "resistance-turned-support" : "support-turned-resistance")} " +
//                           $"at {retestLevel:F4} on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["RetestLevel"] = retestLevel.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates an oversold entry signal
//        /// </summary>
//        public static EntrySignal CreateOversold(
//            string pair,
//            decimal currentPrice,
//            decimal rsiValue,
//            Timeframe timeframe)
//        {
//            return new EntrySignal(EntrySignalType.Oversold, currentPrice, currentPrice, TradeDirection.Long)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                IsImmediate = true,
//                Description = $"Oversold entry (RSI: {rsiValue:F1})",
//                Rationale = $"RSI showing oversold conditions at {rsiValue:F1} " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["RSI"] = rsiValue.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }

//        /// <summary>
//        /// Creates an overbought entry signal (for short)
//        /// </summary>
//        public static EntrySignal CreateOverbought(
//            string pair,
//            decimal currentPrice,
//            decimal rsiValue,
//            Timeframe timeframe)
//        {
//            return new EntrySignal(EntrySignalType.Overbought, currentPrice, currentPrice, TradeDirection.Short)
//            {
//                Pair = pair,
//                Timeframe = timeframe,
//                IsImmediate = true,
//                Description = $"Overbought entry (RSI: {rsiValue:F1})",
//                Rationale = $"RSI showing overbought conditions at {rsiValue:F1} " +
//                           $"on {timeframe.ToDisplayString()} timeframe",
//                Metadata = new Dictionary<string, string>
//                {
//                    ["RSI"] = rsiValue.ToString(),
//                    ["Timeframe"] = timeframe.ToString()
//                }
//            };
//        }
//    }
//}
