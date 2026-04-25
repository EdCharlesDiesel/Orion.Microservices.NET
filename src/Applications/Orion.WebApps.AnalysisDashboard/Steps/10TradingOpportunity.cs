//namespace Orion.WebApps.AanalysisDashboard.Steps
//{

//    /// <summary>
//    /// Represents a complete trading opportunity with entry, exit, and risk parameters
//    /// This is the aggregate root that combines all signal components into an actionable trade setup
//    /// </summary>
//    public sealed class TradingOpportunity
//    {
//        private readonly List<SignalReason> _reasons = new();
//        private readonly Dictionary<Timeframe, TimeframeSignal> _timeframeSignals = new();

//        /// <summary>
//        /// Unique identifier for this trading opportunity
//        /// </summary>
//        public string Id { get; } = Guid.NewGuid().ToString("N");

//        /// <summary>
//        /// Timestamp when this opportunity was generated
//        /// </summary>
//        public DateTime GeneratedAt { get; } = DateTime.UtcNow;

//        /// <summary>
//        /// Trading pair symbol (e.g., EUR/USD)
//        /// </summary>
//        public string Pair { get; set; } = string.Empty;

//        /// <summary>
//        /// Current price at signal generation
//        /// </summary>
//        private decimal _currentPrice;
//        public decimal CurrentPrice
//        {
//            get => _currentPrice;
//            set
//            {
//                if (value <= 0)
//                    throw new ArgumentException("Current price must be greater than zero", nameof(CurrentPrice));
//                _currentPrice = value;
//            }
//        }

//        /// <summary>
//        /// Trading direction (Long/Short)
//        /// </summary>
//        public TradeDirection Direction { get; set; }

//        /// <summary>
//        /// Signal conviction level
//        /// </summary>
//        public ConvictionLevel Conviction { get; set; }

//        /// <summary>
//        /// Composite strength score (0-100)
//        /// </summary>
//        private int _strengthScore;
//        public int StrengthScore
//        {
//            get => _strengthScore;
//            set
//            {
//                if (value < 0 || value > 100)
//                    throw new ArgumentOutOfRangeException(nameof(StrengthScore), "Strength score must be between 0 and 100");
//                _strengthScore = value;
//            }
//        }

//        /// <summary>
//        /// Trading thesis explaining the signal rationale
//        /// </summary>
//        public string Thesis { get; set; } = string.Empty;

//        /// <summary>
//        /// Entry price level
//        /// </summary>
//        private decimal _entryPrice;
//        public decimal EntryPrice
//        {
//            get => _entryPrice;
//            set
//            {
//                if (value <= 0)
//                    throw new ArgumentException("Entry price must be greater than zero", nameof(EntryPrice));
//                _entryPrice = value;
//            }
//        }

//        /// <summary>
//        /// Stop loss price level
//        /// </summary>
//        private decimal _stopLossPrice;
//        public decimal StopLossPrice
//        {
//            get => _stopLossPrice;
//            set
//            {
//                if (value <= 0)
//                    throw new ArgumentException("Stop loss must be greater than zero", nameof(StopLossPrice));
//                _stopLossPrice = value;
//            }
//        }

//        /// <summary>
//        /// Primary take profit level
//        /// </summary>
//        private decimal _takeProfit1Price;
//        public decimal TakeProfit1Price
//        {
//            get => _takeProfit1Price;
//            set
//            {
//                if (value <= 0)
//                    throw new ArgumentException("Take profit 1 must be greater than zero", nameof(TakeProfit1Price));
//                _takeProfit1Price = value;
//            }
//        }

//        /// <summary>
//        /// Secondary take profit level (optional)
//        /// </summary>
//        public decimal? TakeProfit2Price { get; set; }

//        /// <summary>
//        /// Risk/Reward ratio for primary target
//        /// </summary>
//        public decimal RiskRewardRatio { get; set; }

//        /// <summary>
//        /// RSI value at signal generation
//        /// </summary>
//        public decimal? RsiValue { get; set; }

//        /// <summary>
//        /// ATR value for volatility measurement
//        /// </summary>
//        public decimal? AtrValue { get; set; }

//        /// <summary>
//        /// Number of timeframes confirming the signal
//        /// </summary>
//        public int ConfirmingTimeframes { get; set; }

//        /// <summary>
//        /// Primary timeframe that generated the signal
//        /// </summary>
//        public Timeframe PrimaryTimeframe { get; set; }

//        /// <summary>
//        /// Entry signal details if applicable
//        /// </summary>
//        public EntrySignal? EntrySignalDetails { get; set; }

//        /// <summary>
//        /// Position size recommendation (as percentage of account)
//        /// </summary>
//        public decimal RecommendedPositionSize { get; set; }

//        /// <summary>
//        /// Expected holding period
//        /// </summary>
//        public TimeSpan ExpectedHoldingPeriod { get; set; }

//        /// <summary>
//        /// Signal expiration time
//        /// </summary>
//        public DateTime ExpiresAt { get; set; }

//        /// <summary>
//        /// Risk profile used for this opportunity
//        /// </summary>
//        public RiskProfile RiskProfile { get; set; } = RiskProfile.Moderate;

//        /// <summary>
//        /// Read-only collection of reasons supporting the signal
//        /// </summary>
//        public IReadOnlyList<SignalReason> Reasons => _reasons.AsReadOnly();

//        /// <summary>
//        /// Read-only dictionary of signals per timeframe
//        /// </summary>
//        public IReadOnlyDictionary<Timeframe, TimeframeSignal> TimeframeSignals => _timeframeSignals;

//        /// <summary>
//        /// Whether the signal is still valid
//        /// </summary>
//        public bool IsValid =>
//            Direction != TradeDirection.Neutral &&
//            EntryPrice > 0 &&
//            StopLossPrice > 0 &&
//            DateTime.UtcNow < ExpiresAt &&
//            ValidatePriceLevels() &&
//            StrengthScore >= 40;

//        /// <summary>
//        /// Whether the signal has high conviction
//        /// </summary>
//        public bool IsHighConviction => Conviction >= ConvictionLevel.High;

//        /// <summary>
//        /// Whether this opportunity is suitable for trading
//        /// </summary>
//        public bool IsTradeable => IsValid &&
//            Conviction >= RiskProfile.GetMinimumConviction() &&
//            RiskRewardRatio >= RiskProfile.GetMinimumRiskRewardRatio();

//        /// <summary>
//        /// Risk percentage of account
//        /// </summary>
//        public decimal RiskPercentage =>
//            Direction == TradeDirection.Long
//                ? ((EntryPrice - StopLossPrice) / EntryPrice) * 100
//                : ((StopLossPrice - EntryPrice) / EntryPrice) * 100;

//        /// <summary>
//        /// Potential profit percentage for primary target
//        /// </summary>
//        public decimal PotentialProfitPercentage =>
//            Direction == TradeDirection.Long
//                ? ((TakeProfit1Price - EntryPrice) / EntryPrice) * 100
//                : ((EntryPrice - TakeProfit1Price) / EntryPrice) * 100;

//        /// <summary>
//        /// Stop loss distance in pips
//        /// </summary>
//        public decimal StopLossPips => Math.Abs(EntryPrice - StopLossPrice) / 0.0001m;

//        /// <summary>
//        /// Take profit distance in pips
//        /// </summary>
//        public decimal TakeProfitPips => Math.Abs(TakeProfit1Price - EntryPrice) / 0.0001m;

//        /// <summary>
//        /// Internal constructor for builder pattern
//        /// </summary>
//        internal TradingOpportunity()
//        {
//            ExpiresAt = DateTime.UtcNow.AddHours(24);
//            ExpectedHoldingPeriod = TimeSpan.FromDays(3);
//        }

//        /// <summary>
//        /// Creates a new trading opportunity using the builder pattern
//        /// </summary>
//        public static TradingOpportunityBuilder CreateBuilder() => new();

//        /// <summary>
//        /// Adds a reason to the signal (internal use only)
//        /// </summary>
//        internal void AddReason(SignalReason reason)
//        {
//            if (reason != null)
//                _reasons.Add(reason);
//        }

//        /// <summary>
//        /// Adds a timeframe signal (internal use only)
//        /// </summary>
//        internal void AddTimeframeSignal(Timeframe timeframe, TimeframeSignal signal)
//        {
//            if (signal != null)
//                _timeframeSignals[timeframe] = signal;
//        }

//        /// <summary>
//        /// Validates price levels for logical consistency
//        /// </summary>
//        public bool ValidatePriceLevels()
//        {
//            return Direction switch
//            {
//                TradeDirection.Long =>
//                    EntryPrice > StopLossPrice &&
//                    TakeProfit1Price > EntryPrice &&
//                    (TakeProfit2Price == null || TakeProfit2Price > TakeProfit1Price),

//                TradeDirection.Short =>
//                    StopLossPrice > EntryPrice &&
//                    EntryPrice > TakeProfit1Price &&
//                    (TakeProfit2Price == null || TakeProfit1Price > TakeProfit2Price),

//                _ => false
//            };
//        }

//        /// <summary>
//        /// Calculates the position size in lots based on account size and risk percentage
//        /// </summary>
//        public decimal CalculatePositionSize(decimal accountBalance, decimal? riskPerTradeOverride = null)
//        {
//            var riskPerTrade = riskPerTradeOverride ?? RiskProfile.GetRiskPerTrade();
//            var riskAmount = accountBalance * (riskPerTrade / 100);
//            var pipRisk = StopLossPips;

//            return pipRisk > 0 ? riskAmount / (pipRisk * 10000) : 0;
//        }

//        /// <summary>
//        /// Gets the signal as a formatted string
//        /// </summary>
//        public override string ToString()
//        {
//            var direction = Direction == TradeDirection.Long ? "📈 LONG" : "📉 SHORT";
//            var conviction = Conviction.ToString().ToUpper();
//            var tfInfo = ConfirmingTimeframes > 0 ? $" ({ConfirmingTimeframes} TFs)" : "";

//            return $"{Pair}: {direction} [{conviction}]{tfInfo} | " +
//                   $"Entry: {EntryPrice:F4} | SL: {StopLossPrice:F4} | " +
//                   $"TP1: {TakeProfit1Price:F4} | R:R 1:{RiskRewardRatio:F2} | Score: {StrengthScore}/100";
//        }

//        /// <summary>
//        /// Gets a detailed analysis of the signal
//        /// </summary>
//        public string GetDetailedAnalysis()
//        {
//            var lines = new List<string>
//            {
//                ToString(),
//                $"ID: {Id}",
//                $"Generated: {GeneratedAt:yyyy-MM-dd HH:mm} UTC",
//                $"Expires: {ExpiresAt:yyyy-MM-dd HH:mm} UTC",
//                $"Thesis: {Thesis}",
//                $"Price: {CurrentPrice:F4} | RSI: {RsiValue:F1} | ATR: {AtrValue:F4}",
//                $"Risk: {RiskPercentage:F2}% | Reward: {PotentialProfitPercentage:F2}%",
//                $"Stop Loss: {StopLossPips:F0} pips | Take Profit: {TakeProfitPips:F0} pips",
//                $"Position Size: {RecommendedPositionSize:F2}%",
//                $"Risk Profile: {RiskProfile.ToEmoji()} {RiskProfile.ToDisplayString()}",
//                "",
//                "Timeframe Analysis:"
//            };

//            foreach (var tf in _timeframeSignals.OrderByDescending(kvp => kvp.Key))
//            {
//                lines.Add($"  {tf.Value.ToSummary()}");
//            }

//            if (_reasons.Any())
//            {
//                lines.Add("");
//                lines.Add("Supporting Factors:");
//                foreach (var reason in _reasons.OrderByDescending(r => r.Weight))
//                {
//                    lines.Add($"  • {reason.ToDisplayString()}");
//                }
//            }

//            if (EntrySignalDetails != null)
//            {
//                lines.Add("");
//                lines.Add("Entry Signal Details:");
//                lines.Add($"  {EntrySignalDetails}");
//            }

//            return string.Join(Environment.NewLine, lines);
//        }

//        /// <summary>
//        /// Creates a summary suitable for notifications
//        /// </summary>
//        public string ToNotificationSummary()
//        {
//            var direction = Direction == TradeDirection.Long ? "LONG" : "SHORT";
//            return $"🎯 {Pair} {direction} [{Conviction}] | Entry: {EntryPrice:F4} | " +
//                   $"SL: {StopLossPrice:F4} | TP: {TakeProfit1Price:F4} | R:R 1:{RiskRewardRatio:F2}";
//        }
//    }

//    /// <summary>
//    /// Builder class for creating TradingOpportunity instances
//    /// </summary>
//    public class TradingOpportunityBuilder
//    {
//        private readonly TradingOpportunity _opportunity = new();

//        public TradingOpportunityBuilder WithPair(string pair)
//        {
//            _opportunity.Pair = pair;
//            return this;
//        }

//        public TradingOpportunityBuilder WithCurrentPrice(decimal price)
//        {
//            _opportunity.CurrentPrice = price;
//            return this;
//        }

//        public TradingOpportunityBuilder WithDirection(TradeDirection direction)
//        {
//            _opportunity.Direction = direction;
//            return this;
//        }

//        public TradingOpportunityBuilder WithConviction(ConvictionLevel conviction)
//        {
//            _opportunity.Conviction = conviction;
//            return this;
//        }

//        public TradingOpportunityBuilder WithStrengthScore(int score)
//        {
//            _opportunity.StrengthScore = Math.Clamp(score, 0, 100);
//            return this;
//        }

//        public TradingOpportunityBuilder WithThesis(string thesis)
//        {
//            _opportunity.Thesis = thesis;
//            return this;
//        }

//        public TradingOpportunityBuilder WithEntryPrice(decimal entry)
//        {
//            _opportunity.EntryPrice = entry;
//            return this;
//        }

//        public TradingOpportunityBuilder WithStopLoss(decimal stopLoss)
//        {
//            _opportunity.StopLossPrice = stopLoss;
//            return this;
//        }

//        public TradingOpportunityBuilder WithTakeProfit1(decimal tp1)
//        {
//            _opportunity.TakeProfit1Price = tp1;
//            return this;
//        }

//        public TradingOpportunityBuilder WithTakeProfit2(decimal? tp2)
//        {
//            _opportunity.TakeProfit2Price = tp2;
//            return this;
//        }

//        public TradingOpportunityBuilder WithRiskRewardRatio(decimal? ratio = null)
//        {
//            if (ratio.HasValue)
//            {
//                _opportunity.RiskRewardRatio = ratio.Value;
//            }
//            else
//            {
//                CalculateRiskReward();
//            }
//            return this;
//        }

//        public TradingOpportunityBuilder WithRsi(decimal? rsi)
//        {
//            _opportunity.RsiValue = rsi;
//            return this;
//        }

//        public TradingOpportunityBuilder WithAtr(decimal? atr)
//        {
//            _opportunity.AtrValue = atr;
//            return this;
//        }

//        public TradingOpportunityBuilder WithConfirmingTimeframes(int count)
//        {
//            _opportunity.ConfirmingTimeframes = count;
//            return this;
//        }

//        public TradingOpportunityBuilder WithPrimaryTimeframe(Timeframe timeframe)
//        {
//            _opportunity.PrimaryTimeframe = timeframe;
//            return this;
//        }

//        public TradingOpportunityBuilder WithEntrySignal(EntrySignal entrySignal)
//        {
//            _opportunity.EntrySignalDetails = entrySignal;
//            return this;
//        }

//        public TradingOpportunityBuilder WithRecommendedPositionSize(decimal size)
//        {
//            _opportunity.RecommendedPositionSize = size;
//            return this;
//        }

//        public TradingOpportunityBuilder WithExpectedHoldingPeriod(TimeSpan period)
//        {
//            _opportunity.ExpectedHoldingPeriod = period;
//            return this;
//        }

//        public TradingOpportunityBuilder WithExpiration(DateTime expiration)
//        {
//            _opportunity.ExpiresAt = expiration;
//            return this;
//        }

//        public TradingOpportunityBuilder WithRiskProfile(RiskProfile profile)
//        {
//            _opportunity.RiskProfile = profile;
//            return this;
//        }

//        public TradingOpportunityBuilder AddReason(string description, int weight, SignalCategory category)
//        {
//            _opportunity.AddReason(new SignalReason(description, weight, category));
//            return this;
//        }

//        public TradingOpportunityBuilder AddReason(SignalReason reason)
//        {
//            _opportunity.AddReason(reason);
//            return this;
//        }

//        public TradingOpportunityBuilder AddReasons(IEnumerable<SignalReason> reasons)
//        {
//            foreach (var reason in reasons)
//                _opportunity.AddReason(reason);
//            return this;
//        }

//        public TradingOpportunityBuilder AddTimeframeSignal(Timeframe timeframe, TimeframeSignal signal)
//        {
//            _opportunity.AddTimeframeSignal(timeframe, signal);
//            return this;
//        }

//        public TradingOpportunityBuilder AddTimeframeSignals(Dictionary<Timeframe, TimeframeSignal> signals)
//        {
//            foreach (var kvp in signals)
//                _opportunity.AddTimeframeSignal(kvp.Key, kvp.Value);
//            return this;
//        }

//        /// <summary>
//        /// Automatically calculates R:R ratio based on price levels
//        /// </summary>
//        public TradingOpportunityBuilder CalculateRiskReward()
//        {
//            if (_opportunity.EntryPrice > 0 &&
//                _opportunity.StopLossPrice > 0 &&
//                _opportunity.TakeProfit1Price > 0)
//            {
//                var risk = Math.Abs(_opportunity.EntryPrice - _opportunity.StopLossPrice);
//                var reward = Math.Abs(_opportunity.TakeProfit1Price - _opportunity.EntryPrice);
//                _opportunity.RiskRewardRatio = risk > 0 ? reward / risk : 0;
//            }
//            return this;
//        }

//        /// <summary>
//        /// Builds and validates the TradingOpportunity
//        /// </summary>
//        public TradingOpportunity Build()
//        {
//            Validate();
//            return _opportunity;
//        }

//        private void Validate()
//        {
//            var errors = new List<string>();

//            if (string.IsNullOrWhiteSpace(_opportunity.Pair))
//                errors.Add("Pair is required");

//            if (_opportunity.Direction == TradeDirection.Neutral)
//                errors.Add("Trade direction must be specified");

//            if (_opportunity.EntryPrice <= 0)
//                errors.Add("Entry price must be greater than 0");

//            if (_opportunity.StopLossPrice <= 0)
//                errors.Add("Stop loss must be greater than 0");

//            if (_opportunity.TakeProfit1Price <= 0)
//                errors.Add("Take profit must be greater than 0");

//            if (!_opportunity.ValidatePriceLevels())
//                errors.Add("Price levels are not logically consistent with trade direction");

//            if (_opportunity.StrengthScore == 0)
//                errors.Add("Strength score must be calculated");

//            if (errors.Any())
//                throw new InvalidOperationException($"Invalid trading opportunity: {string.Join(", ", errors)}");
//        }
//    }
//}
