//namespace Orion.WebApps.AanalysisDashboard.Steps
//{


//    /// <summary>
//    /// Fluent builder for constructing TradingOpportunity instances
//    /// Provides a clean API for creating complex trading signals with validation
//    /// </summary>
//    public class TradingSignalBuilder
//    {
//        private readonly TradingOpportunity _opportunity;
//        private readonly List<SignalReason> _pendingReasons = new();
//        private readonly Dictionary<Timeframe, TimeframeSignal> _pendingTimeframeSignals = new();
//        private bool _autoCalculateRiskReward = true;
//        private bool _autoCalculateStrength = true;

//        /// <summary>
//        /// Private constructor - use Create() method
//        /// </summary>
//        private TradingSignalBuilder()
//        {
//            _opportunity = new TradingOpportunity();
//        }

//        /// <summary>
//        /// Creates a new builder instance
//        /// </summary>
//        public static TradingSignalBuilder Create() => new();

//        /// <summary>
//        /// Creates a builder from an existing opportunity
//        /// </summary>
//        public static TradingSignalBuilder From(TradingOpportunity existing)
//        {
//            var builder = new TradingSignalBuilder();

//            builder.WithPair(existing.Pair)
//                   .WithCurrentPrice(existing.CurrentPrice)
//                   .WithDirection(existing.Direction)
//                   .WithConviction(existing.Conviction)
//                   .WithStrengthScore(existing.StrengthScore)
//                   .WithThesis(existing.Thesis)
//                   .WithEntryPrice(existing.EntryPrice)
//                   .WithStopLoss(existing.StopLossPrice)
//                   .WithTakeProfit1(existing.TakeProfit1Price)
//                   .WithRiskProfile(existing.RiskProfile)
//                   .WithConfirmingTimeframes(existing.ConfirmingTimeframes)
//                   .WithPrimaryTimeframe(existing.PrimaryTimeframe);

//            if (existing.TakeProfit2Price.HasValue)
//                builder.WithTakeProfit2(existing.TakeProfit2Price.Value);

//            if (existing.RsiValue.HasValue)
//                builder.WithRsi(existing.RsiValue.Value);

//            if (existing.AtrValue.HasValue)
//                builder.WithAtr(existing.AtrValue.Value);

//            if (existing.EntrySignalDetails != null)
//                builder.WithEntrySignal(existing.EntrySignalDetails);

//            builder.AddReasons(existing.Reasons);
//            builder.AddTimeframeSignals(existing.TimeframeSignals.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

//            return builder;
//        }

//        // ========== Core Properties ==========

//        public TradingSignalBuilder WithPair(string pair)
//        {
//            if (string.IsNullOrWhiteSpace(pair))
//                throw new ArgumentException("Pair cannot be empty", nameof(pair));

//            _opportunity.Pair = pair.ToUpperInvariant();
//            return this;
//        }

//        public TradingSignalBuilder WithCurrentPrice(decimal price)
//        {
//            if (price <= 0)
//                throw new ArgumentException("Current price must be greater than zero", nameof(price));

//            _opportunity.CurrentPrice = price;
//            return this;
//        }

//        public TradingSignalBuilder WithDirection(TradeDirection direction)
//        {
//            if (direction == TradeDirection.Neutral)
//                throw new ArgumentException("Direction must be Long or Short", nameof(direction));

//            _opportunity.Direction = direction;
//            return this;
//        }

//        /// <summary>
//        /// Sets direction from string ("Long", "Short", "Buy", "Sell", etc.)
//        /// </summary>
//        public TradingSignalBuilder WithDirection(string direction)
//        {
//            var parsed = direction.ToLowerInvariant() switch
//            {
//                "long" or "buy" or "bullish" or "1" => TradeDirection.Long,
//                "short" or "sell" or "bearish" or "-1" => TradeDirection.Short,
//                _ => TradeDirection.Neutral
//            };

//            if (parsed == TradeDirection.Neutral)
//                throw new ArgumentException($"Invalid direction: {direction}", nameof(direction));

//            return WithDirection(parsed);
//        }

//        public TradingSignalBuilder WithConviction(ConvictionLevel conviction)
//        {
//            _opportunity.Conviction = conviction;
//            return this;
//        }

//        public TradingSignalBuilder WithStrengthScore(int score)
//        {
//            _opportunity.StrengthScore = Math.Clamp(score, 0, 100);
//            return this;
//        }

//        public TradingSignalBuilder WithThesis(string thesis)
//        {
//            _opportunity.Thesis = thesis ?? string.Empty;
//            return this;
//        }

//        // ========== Price Levels ==========

//        public TradingSignalBuilder WithEntryPrice(decimal entry)
//        {
//            if (entry <= 0)
//                throw new ArgumentException("Entry price must be greater than zero", nameof(entry));

//            _opportunity.EntryPrice = entry;
//            return this;
//        }

//        public TradingSignalBuilder WithStopLoss(decimal stopLoss)
//        {
//            if (stopLoss <= 0)
//                throw new ArgumentException("Stop loss must be greater than zero", nameof(stopLoss));

//            _opportunity.StopLossPrice = stopLoss;
//            return this;
//        }

//        /// <summary>
//        /// Sets stop loss based on ATR multiple
//        /// </summary>
//        public TradingSignalBuilder WithStopLossUsingAtr(decimal atrValue, decimal multiplier = 1.5m)
//        {
//            if (_opportunity.EntryPrice <= 0)
//                throw new InvalidOperationException("Entry price must be set before calculating stop loss");

//            var stopDistance = atrValue * multiplier;

//            var stopLoss = _opportunity.Direction == TradeDirection.Long
//                ? _opportunity.EntryPrice - stopDistance
//                : _opportunity.EntryPrice + stopDistance;

//            return WithStopLoss(stopLoss);
//        }

//        /// <summary>
//        /// Sets stop loss using the risk profile's ATR multiplier
//        /// </summary>
//        public TradingSignalBuilder WithStopLossUsingRiskProfile(decimal atrValue)
//        {
//            var multiplier = _opportunity.RiskProfile.GetStopLossAtrMultiplier();
//            return WithStopLossUsingAtr(atrValue, multiplier);
//        }

//        public TradingSignalBuilder WithTakeProfit1(decimal tp1)
//        {
//            if (tp1 <= 0)
//                throw new ArgumentException("Take profit 1 must be greater than zero", nameof(tp1));

//            _opportunity.TakeProfit1Price = tp1;
//            return this;
//        }

//        /// <summary>
//        /// Sets take profit based on risk/reward ratio
//        /// </summary>
//        public TradingSignalBuilder WithTakeProfit1UsingRR(decimal riskRewardRatio)
//        {
//            if (_opportunity.EntryPrice <= 0 || _opportunity.StopLossPrice <= 0)
//                throw new InvalidOperationException("Entry and stop loss must be set before calculating take profit");

//            var risk = Math.Abs(_opportunity.EntryPrice - _opportunity.StopLossPrice);
//            var reward = risk * riskRewardRatio;

//            var tp1 = _opportunity.Direction == TradeDirection.Long
//                ? _opportunity.EntryPrice + reward
//                : _opportunity.EntryPrice - reward;

//            return WithTakeProfit1(tp1);
//        }

//        public TradingSignalBuilder WithTakeProfit2(decimal tp2)
//        {
//            if (tp2 <= 0)
//                throw new ArgumentException("Take profit 2 must be greater than zero", nameof(tp2));

//            _opportunity.TakeProfit2Price = tp2;
//            return this;
//        }

//        public TradingSignalBuilder WithoutTakeProfit2()
//        {
//            _opportunity.TakeProfit2Price = null;
//            return this;
//        }

//        // ========== Indicators ==========

//        public TradingSignalBuilder WithRsi(decimal rsi)
//        {
//            if (rsi < 0 || rsi > 100)
//                throw new ArgumentOutOfRangeException(nameof(rsi), "RSI must be between 0 and 100");

//            _opportunity.RsiValue = rsi;
//            return this;
//        }

//        public TradingSignalBuilder WithAtr(decimal atr)
//        {
//            if (atr < 0)
//                throw new ArgumentOutOfRangeException(nameof(atr), "ATR cannot be negative");

//            _opportunity.AtrValue = atr;
//            return this;
//        }

//        // ========== Timeframe Analysis ==========

//        public TradingSignalBuilder WithConfirmingTimeframes(int count)
//        {
//            _opportunity.ConfirmingTimeframes = count;
//            return this;
//        }

//        public TradingSignalBuilder WithPrimaryTimeframe(Timeframe timeframe)
//        {
//            _opportunity.PrimaryTimeframe = timeframe;
//            return this;
//        }

//        public TradingSignalBuilder AddTimeframeSignal(Timeframe timeframe, TimeframeSignal signal)
//        {
//            _pendingTimeframeSignals[timeframe] = signal;
//            return this;
//        }

//        public TradingSignalBuilder AddTimeframeSignals(Dictionary<Timeframe, TimeframeSignal> signals)
//        {
//            foreach (var kvp in signals)
//                _pendingTimeframeSignals[kvp.Key] = kvp.Value;
//            return this;
//        }

//        /// <summary>
//        /// Adds a timeframe signal with basic parameters
//        /// </summary>
//        public TradingSignalBuilder AddTimeframeSignal(
//            Timeframe timeframe,
//            TradeDirection direction,
//            int strengthScore,
//            decimal currentPrice,
//            string analysis = null)
//        {
//            var signal = new TimeframeSignal(timeframe, direction, strengthScore, currentPrice)
//            {
//                Analysis = analysis ?? $"{timeframe.ToDisplayString()} shows {direction.ToDisplayString().ToLower()} bias"
//            };

//            return AddTimeframeSignal(timeframe, signal);
//        }

//        // ========== Entry Signal ==========

//        public TradingSignalBuilder WithEntrySignal(EntrySignal entrySignal)
//        {
//            _opportunity.EntrySignalDetails = entrySignal;
//            return this;
//        }

//        /// <summary>
//        /// Creates and adds a breakout entry signal
//        /// </summary>
//        public TradingSignalBuilder WithBreakoutEntry(
//            decimal breakoutLevel,
//            Timeframe timeframe,
//            long? minimumVolume = null)
//        {
//            var entrySignal = EntrySignalFactory.CreateBreakout(
//                _opportunity.Pair,
//                breakoutLevel,
//                _opportunity.CurrentPrice,
//                _opportunity.Direction,
//                timeframe,
//                minimumVolume);

//            return WithEntrySignal(entrySignal);
//        }

//        /// <summary>
//        /// Creates and adds a pullback entry signal
//        /// </summary>
//        public TradingSignalBuilder WithPullbackEntry(decimal pullbackLevel, Timeframe timeframe)
//        {
//            var entrySignal = EntrySignalFactory.CreatePullback(
//                _opportunity.Pair,
//                pullbackLevel,
//                _opportunity.CurrentPrice,
//                _opportunity.Direction,
//                timeframe);

//            return WithEntrySignal(entrySignal);
//        }

//        /// <summary>
//        /// Creates and adds a momentum entry signal
//        /// </summary>
//        public TradingSignalBuilder WithMomentumEntry(Timeframe timeframe, decimal momentumThreshold = 0.002m)
//        {
//            var entrySignal = EntrySignalFactory.CreateMomentum(
//                _opportunity.Pair,
//                _opportunity.CurrentPrice,
//                _opportunity.Direction,
//                timeframe,
//                momentumThreshold);

//            return WithEntrySignal(entrySignal);
//        }

//        // ========== Position Sizing ==========

//        public TradingSignalBuilder WithRecommendedPositionSize(decimal size)
//        {
//            if (size < 0 || size > 100)
//                throw new ArgumentOutOfRangeException(nameof(size), "Position size must be between 0 and 100");

//            _opportunity.RecommendedPositionSize = size;
//            return this;
//        }

//        /// <summary>
//        /// Calculates recommended position size based on account balance
//        /// </summary>
//        public TradingSignalBuilder CalculatePositionSize(decimal accountBalance)
//        {
//            var size = _opportunity.CalculatePositionSize(accountBalance);
//            return WithRecommendedPositionSize(size);
//        }

//        // ========== Risk Management ==========

//        public TradingSignalBuilder WithRiskProfile(RiskProfile profile)
//        {
//            _opportunity.RiskProfile = profile;
//            return this;
//        }

//        public TradingSignalBuilder WithExpectedHoldingPeriod(TimeSpan period)
//        {
//            _opportunity.ExpectedHoldingPeriod = period;
//            return this;
//        }

//        public TradingSignalBuilder WithExpiration(DateTime expiration)
//        {
//            if (expiration <= DateTime.UtcNow)
//                throw new ArgumentException("Expiration must be in the future", nameof(expiration));

//            _opportunity.ExpiresAt = expiration;
//            return this;
//        }

//        /// <summary>
//        /// Sets expiration based on holding period
//        /// </summary>
//        public TradingSignalBuilder WithExpirationFromHoldingPeriod()
//        {
//            return WithExpiration(DateTime.UtcNow.Add(_opportunity.ExpectedHoldingPeriod));
//        }

//        /// <summary>
//        /// Sets expiration based on timeframe
//        /// </summary>
//        public TradingSignalBuilder WithExpirationFromTimeframe()
//        {
//            var days = _opportunity.PrimaryTimeframe.GetTypicalLookbackDays();
//            return WithExpiration(DateTime.UtcNow.AddDays(days));
//        }

//        // ========== Reasons ==========

//        public TradingSignalBuilder AddReason(string description, int weight, SignalCategory category)
//        {
//            _pendingReasons.Add(new SignalReason(description, weight, category));
//            return this;
//        }

//        public TradingSignalBuilder AddReason(SignalReason reason)
//        {
//            _pendingReasons.Add(reason);
//            return this;
//        }

//        public TradingSignalBuilder AddReasons(IEnumerable<SignalReason> reasons)
//        {
//            _pendingReasons.AddRange(reasons);
//            return this;
//        }

//        /// <summary>
//        /// Adds an RSI-based reason
//        /// </summary>
//        public TradingSignalBuilder AddRsiReason(decimal rsiValue, Timeframe timeframe)
//        {
//            if (rsiValue < 30)
//            {
//                _pendingReasons.Add(SignalReasonFactory.CreateRsiOversold(rsiValue, timeframe));
//            }
//            else if (rsiValue > 70)
//            {
//                _pendingReasons.Add(SignalReasonFactory.CreateRsiOverbought(rsiValue, timeframe));
//            }
//            return this;
//        }

//        /// <summary>
//        /// Adds a trend alignment reason
//        /// </summary>
//        public TradingSignalBuilder AddTrendAlignmentReason(int confirmingTimeframes)
//        {
//            _pendingReasons.Add(SignalReasonFactory.CreateTrendAlignment(
//                confirmingTimeframes,
//                _opportunity.Direction));
//            return this;
//        }

//        /// <summary>
//        /// Adds a volume spike reason
//        /// </summary>
//        public TradingSignalBuilder AddVolumeSpikeReason(decimal volumeRatio, Timeframe timeframe)
//        {
//            _pendingReasons.Add(SignalReasonFactory.CreateVolumeSpike(
//                volumeRatio,
//                _opportunity.Direction,
//                timeframe));
//            return this;
//        }

//        /// <summary>
//        /// Adds a divergence reason
//        /// </summary>
//        public TradingSignalBuilder AddDivergenceReason(string indicator, Timeframe timeframe)
//        {
//            _pendingReasons.Add(SignalReasonFactory.CreateDivergence(
//                indicator,
//                _opportunity.Direction,
//                timeframe));
//            return this;
//        }

//        // ========== Builder Configuration ==========

//        /// <summary>
//        /// Disables automatic risk/reward calculation
//        /// </summary>
//        public TradingSignalBuilder WithoutAutoRiskReward()
//        {
//            _autoCalculateRiskReward = false;
//            return this;
//        }

//        /// <summary>
//        /// Disables automatic strength score calculation
//        /// </summary>
//        public TradingSignalBuilder WithoutAutoStrength()
//        {
//            _autoCalculateStrength = false;
//            return this;
//        }

//        // ========== Build ==========

//        /// <summary>
//        /// Builds and returns the TradingOpportunity
//        /// </summary>
//        public TradingOpportunity Build()
//        {
//            // Apply pending reasons
//            foreach (var reason in _pendingReasons)
//                _opportunity.AddReason(reason);

//            // Apply pending timeframe signals
//            foreach (var kvp in _pendingTimeframeSignals)
//                _opportunity.AddTimeframeSignal(kvp.Key, kvp.Value);

//            // Auto-calculate confirming timeframes if not set
//            if (_opportunity.ConfirmingTimeframes == 0 && _pendingTimeframeSignals.Any())
//            {
//                var confirming = _pendingTimeframeSignals.Values
//                    .Count(s => s.ConfirmsDirection(_opportunity.Direction));
//                _opportunity.ConfirmingTimeframes = confirming;
//            }

//            // Auto-calculate risk/reward if enabled
//            if (_autoCalculateRiskReward && _opportunity.RiskRewardRatio == 0)
//            {
//                CalculateRiskReward();
//            }

//            // Auto-calculate strength score if enabled
//            if (_autoCalculateStrength && _opportunity.StrengthScore == 0)
//            {
//                CalculateStrengthScore();
//            }

//            // Set default expiration if not specified
//            if (_opportunity.ExpiresAt == default)
//            {
//                _opportunity.ExpiresAt = DateTime.UtcNow.AddHours(24);
//            }

//            // Set default holding period if not specified
//            if (_opportunity.ExpectedHoldingPeriod == default)
//            {
//                _opportunity.ExpectedHoldingPeriod = _opportunity.PrimaryTimeframe.GetTypicalTimeHorizon();
//            }

//            // Validate the opportunity
//            Validate();

//            return _opportunity;
//        }

//        private void CalculateRiskReward()
//        {
//            if (_opportunity.EntryPrice > 0 &&
//                _opportunity.StopLossPrice > 0 &&
//                _opportunity.TakeProfit1Price > 0)
//            {
//                var risk = Math.Abs(_opportunity.EntryPrice - _opportunity.StopLossPrice);
//                var reward = Math.Abs(_opportunity.TakeProfit1Price - _opportunity.EntryPrice);
//                _opportunity.RiskRewardRatio = risk > 0 ? reward / risk : 0;
//            }
//        }

//        private void CalculateStrengthScore()
//        {
//            var score = 50; // Base score

//            // Directional strength from reasons
//            var bullishWeight = _pendingReasons
//                .Where(r => r.Direction == TradeDirection.Long)
//                .Sum(r => r.GetEffectiveWeight());

//            var bearishWeight = _pendingReasons
//                .Where(r => r.Direction == TradeDirection.Short)
//                .Sum(r => r.GetEffectiveWeight());

//            if (_opportunity.Direction == TradeDirection.Long)
//                score += (int)(bullishWeight * 5);
//            else if (_opportunity.Direction == TradeDirection.Short)
//                score += (int)(bearishWeight * 5);

//            // Timeframe confirmation bonus
//            score += _opportunity.ConfirmingTimeframes * 5;

//            // R:R bonus
//            if (_opportunity.RiskRewardRatio >= 3)
//                score += 15;
//            else if (_opportunity.RiskRewardRatio >= 2)
//                score += 10;
//            else if (_opportunity.RiskRewardRatio >= 1.5m)
//                score += 5;

//            _opportunity.StrengthScore = Math.Clamp(score, 0, 100);
//        }

//        private void Validate()
//        {
//            var errors = new List<string>();

//            if (string.IsNullOrWhiteSpace(_opportunity.Pair))
//                errors.Add("Pair is required");

//            if (_opportunity.Direction == TradeDirection.Neutral)
//                errors.Add("Trade direction must be specified");

//            if (_opportunity.CurrentPrice <= 0)
//                errors.Add("Current price must be set");

//            if (_opportunity.EntryPrice <= 0)
//                errors.Add("Entry price must be set");

//            if (_opportunity.StopLossPrice <= 0)
//                errors.Add("Stop loss must be set");

//            if (_opportunity.TakeProfit1Price <= 0)
//                errors.Add("Take profit 1 must be set");

//            if (_opportunity.StrengthScore == 0)
//                errors.Add("Strength score could not be calculated");

//            // Validate price level logic
//            if (_opportunity.Direction == TradeDirection.Long)
//            {
//                if (_opportunity.StopLossPrice >= _opportunity.EntryPrice)
//                    errors.Add("For Long trades, stop loss must be below entry price");
//                if (_opportunity.TakeProfit1Price <= _opportunity.EntryPrice)
//                    errors.Add("For Long trades, take profit must be above entry price");
//            }
//            else if (_opportunity.Direction == TradeDirection.Short)
//            {
//                if (_opportunity.StopLossPrice <= _opportunity.EntryPrice)
//                    errors.Add("For Short trades, stop loss must be above entry price");
//                if (_opportunity.TakeProfit1Price >= _opportunity.EntryPrice)
//                    errors.Add("For Short trades, take profit must be below entry price");
//            }

//            if (errors.Any())
//                throw new InvalidOperationException($"Invalid trading opportunity: {string.Join("; ", errors)}");
//        }
//    }
//}
