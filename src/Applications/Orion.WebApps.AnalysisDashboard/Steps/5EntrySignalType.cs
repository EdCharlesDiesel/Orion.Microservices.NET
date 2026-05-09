//namespace Orion.WebApps.AanalysisDashboard.Steps
//{
    
//        /// <summary>
//        /// Represents the type of entry trigger for trade execution
//        /// Defines the specific market condition that signals trade entry
//        /// </summary>
//        public enum EntrySignalType
//        {
//            /// <summary>
//            /// Price breaking above resistance or below support with momentum
//            /// </summary>
//            Breakout = 1,

//            /// <summary>
//            /// Price pulling back to a key level within an established trend
//            /// </summary>
//            Pullback = 2,

//            /// <summary>
//            /// Trend reversal at exhaustion points
//            /// </summary>
//            Reversal = 3,

//            /// <summary>
//            /// Continuation of an existing trend after consolidation
//            /// </summary>
//            Continuation = 4,

//            /// <summary>
//            /// Price bouncing off a confirmed support level
//            /// </summary>
//            SupportBounce = 5,

//            /// <summary>
//            /// Price breaking through a confirmed resistance level
//            /// </summary>
//            ResistanceBreak = 6,

//            /// <summary>
//            /// Completion of a recognized chart pattern
//            /// </summary>
//            PatternCompletion = 7,

//            /// <summary>
//            /// Moving average crossover signal
//            /// </summary>
//            MaCross = 8,

//            /// <summary>
//            /// Divergence between price and oscillator
//            /// </summary>
//            Divergence = 9,

//            /// <summary>
//            /// Oversold condition (for long entries)
//            /// </summary>
//            Oversold = 10,

//            /// <summary>
//            /// Overbought condition (for short entries)
//            /// </summary>
//            Overbought = 11,

//            /// <summary>
//            /// Retest of a previously broken level
//            /// </summary>
//            Retest = 12,

//            /// <summary>
//            /// Unusual volume spike indicating institutional activity
//            /// </summary>
//            VolumeSpike = 13,

//            /// <summary>
//            /// News or economic event-driven entry
//            /// </summary>
//            NewsCatalyst = 14,

//            /// <summary>
//            /// Opening range breakout
//            /// </summary>
//            OpeningRangeBreak = 15,

//            /// <summary>
//            /// Gap fill completion entry
//            /// </summary>
//            GapFill = 16,

//            /// <summary>
//            /// Reaction at Fibonacci retracement level
//            /// </summary>
//            FibonacciLevel = 17,

//            /// <summary>
//            /// Reaction at round number psychological level
//            /// </summary>
//            PsychologicalLevel = 18,

//            /// <summary>
//            /// Entry based on trading session open/close
//            /// </summary>
//            SessionTransition = 19,

//            /// <summary>
//            /// Pure momentum-based entry with strong directional movement
//            /// </summary>
//            Momentum = 20,


//            GetSlippageTolerance = 21
//    }

//        /// <summary>
//        /// Extension methods for EntrySignalType
//        /// </summary>
//        public static class EntrySignalTypeExtensions
//        {
//            /// <summary>
//            /// Returns a human-readable display string
//            /// </summary>
//            public static string ToDisplayString(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => "Breakout",
//                    EntrySignalType.Pullback => "Pullback",
//                    EntrySignalType.Reversal => "Reversal",
//                    EntrySignalType.Continuation => "Continuation",
//                    EntrySignalType.SupportBounce => "Support Bounce",
//                    EntrySignalType.ResistanceBreak => "Resistance Break",
//                    EntrySignalType.PatternCompletion => "Pattern Completion",
//                    EntrySignalType.MaCross => "MA Crossover",
//                    EntrySignalType.Divergence => "Divergence",
//                    EntrySignalType.Oversold => "Oversold",
//                    EntrySignalType.Overbought => "Overbought",
//                    EntrySignalType.Retest => "Retest",
//                    EntrySignalType.VolumeSpike => "Volume Spike",
//                    EntrySignalType.NewsCatalyst => "News Catalyst",
//                    EntrySignalType.OpeningRangeBreak => "Opening Range Break",
//                    EntrySignalType.GapFill => "Gap Fill",
//                    EntrySignalType.FibonacciLevel => "Fibonacci Level",
//                    EntrySignalType.PsychologicalLevel => "Psychological Level",
//                    EntrySignalType.SessionTransition => "Session Transition",
//                    EntrySignalType.Momentum => "Momentum",
//                    _ => "Unknown"
//                };
//            }

//            /// <summary>
//            /// Returns an emoji representation for UI display
//            /// </summary>
//            public static string ToEmoji(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => "🚀",
//                    EntrySignalType.Pullback => "↩️",
//                    EntrySignalType.Reversal => "🔄",
//                    EntrySignalType.Continuation => "➡️",
//                    EntrySignalType.SupportBounce => "⬆️",
//                    EntrySignalType.ResistanceBreak => "⬆️",
//                    EntrySignalType.PatternCompletion => "✅",
//                    EntrySignalType.MaCross => "📊",
//                    EntrySignalType.Divergence => "↗️",
//                    EntrySignalType.Oversold => "💚",
//                    EntrySignalType.Overbought => "❤️",
//                    EntrySignalType.Retest => "🔁",
//                    EntrySignalType.VolumeSpike => "📈",
//                    EntrySignalType.NewsCatalyst => "📰",
//                    EntrySignalType.OpeningRangeBreak => "🌅",
//                    EntrySignalType.GapFill => "🔲",
//                    EntrySignalType.FibonacciLevel => "🌀",
//                    EntrySignalType.PsychologicalLevel => "💯",
//                    EntrySignalType.SessionTransition => "🌍",
//                    EntrySignalType.Momentum => "⚡",
//                    _ => "📌"
//                };
//            }

//            /// <summary>
//            /// Returns true if this entry type is aggressive (chasing price)
//            /// </summary>
//            public static bool IsAggressive(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => true,
//                    EntrySignalType.ResistanceBreak => true,
//                    EntrySignalType.OpeningRangeBreak => true,
//                    EntrySignalType.Momentum => true,
//                    EntrySignalType.NewsCatalyst => true,
//                    _ => false
//                };
//            }

//            /// <summary>
//            /// Returns true if this entry type is conservative (waiting for pullback/confirmation)
//            /// </summary>
//            public static bool IsConservative(this EntrySignalType type)
//            {
//                return !type.IsAggressive();
//            }

//            /// <summary>
//            /// Returns the typical risk/reward expectation for this entry type
//            /// </summary>
//            public static decimal GetTypicalRiskRewardRatio(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => 2.0m,
//                    EntrySignalType.Pullback => 3.0m,
//                    EntrySignalType.Reversal => 3.5m,
//                    EntrySignalType.Continuation => 2.5m,
//                    EntrySignalType.SupportBounce => 2.5m,
//                    EntrySignalType.ResistanceBreak => 2.0m,
//                    EntrySignalType.PatternCompletion => 2.5m,
//                    EntrySignalType.MaCross => 2.0m,
//                    EntrySignalType.Divergence => 3.0m,
//                    EntrySignalType.Oversold => 2.5m,
//                    EntrySignalType.Overbought => 2.5m,
//                    EntrySignalType.Retest => 3.0m,
//                    EntrySignalType.VolumeSpike => 2.5m,
//                    EntrySignalType.FibonacciLevel => 2.5m,
//                    EntrySignalType.Momentum => 1.8m,
//                    _ => 2.0m
//                };
//            }

//            /// <summary>
//            /// Returns the typical win rate expectation for this entry type
//            /// </summary>
//            public static decimal GetTypicalWinRate(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Pullback => 65m,
//                    EntrySignalType.Retest => 60m,
//                    EntrySignalType.SupportBounce => 55m,
//                    EntrySignalType.Breakout => 45m,
//                    EntrySignalType.Reversal => 40m,
//                    EntrySignalType.MaCross => 50m,
//                    EntrySignalType.Divergence => 55m,
//                    EntrySignalType.Momentum => 48m,
//                    _ => 50m
//                };
//            }

//            /// <summary>
//            /// Returns the recommended confirmation method for this entry type
//            /// </summary>
//            public static string GetConfirmationMethod(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => "Wait for candle close beyond level with volume confirmation",
//                    EntrySignalType.Pullback => "Look for rejection wick or reversal candle pattern",
//                    EntrySignalType.Reversal => "Confirm with divergence and multiple timeframe alignment",
//                    EntrySignalType.SupportBounce => "Confirm with volume increase and bullish candle",
//                    EntrySignalType.ResistanceBreak => "Confirm with retest of broken level",
//                    EntrySignalType.PatternCompletion => "Wait for pattern target projection and volume",
//                    EntrySignalType.MaCross => "Confirm with price above/below moving averages",
//                    EntrySignalType.Divergence => "Wait for price confirmation candle",
//                    EntrySignalType.Momentum => "Confirm sustained directional movement with increasing volume",
//                    _ => "Use standard price action confirmation"
//                };
//            }

//            /// <summary>
//            /// Returns the complementary direction typically associated with this entry type
//            /// </summary>
//            public static TradeDirection GetTypicalDirection(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Oversold => TradeDirection.Long,
//                    EntrySignalType.SupportBounce => TradeDirection.Long,
//                    EntrySignalType.Overbought => TradeDirection.Short,
//                    EntrySignalType.ResistanceBreak => TradeDirection.Long,
//                    _ => TradeDirection.Neutral
//                };
//            }

//            /// <summary>
//            /// Returns a detailed description of the entry trigger
//            /// </summary>
//            public static string GetDescription(this EntrySignalType type)
//            {
//                return type switch
//                {
//                    EntrySignalType.Breakout => "Price breaking through established support/resistance with momentum",
//                    EntrySignalType.Pullback => "Price retracing to key level within established trend",
//                    EntrySignalType.Reversal => "Trend exhaustion with potential directional change",
//                    EntrySignalType.Continuation => "Trend resuming after consolidation period",
//                    EntrySignalType.SupportBounce => "Price rejecting confirmed support level",
//                    EntrySignalType.ResistanceBreak => "Price breaking above established resistance",
//                    EntrySignalType.PatternCompletion => "Chart pattern reaching measured move target",
//                    EntrySignalType.MaCross => "Moving average crossover generating signal",
//                    EntrySignalType.Divergence => "Oscillator diverging from price action",
//                    EntrySignalType.Oversold => "Price reaching oversold extreme on oscillator",
//                    EntrySignalType.Overbought => "Price reaching overbought extreme on oscillator",
//                    EntrySignalType.Retest => "Price testing previously broken support/resistance",
//                    EntrySignalType.VolumeSpike => "Unusual volume indicating institutional participation",
//                    EntrySignalType.NewsCatalyst => "News event creating trading opportunity",
//                    EntrySignalType.OpeningRangeBreak => "Price breaking initial trading range after open",
//                    EntrySignalType.GapFill => "Price returning to fill previous price gap",
//                    EntrySignalType.FibonacciLevel => "Price reacting at key Fibonacci retracement",
//                    EntrySignalType.PsychologicalLevel => "Price reacting at round number level",
//                    EntrySignalType.SessionTransition => "Entry based on session open/close dynamics",
//                    EntrySignalType.Momentum => "Strong directional price movement with momentum confirmation",
//                    _ => "Standard entry signal"
//                };
//            }

//            /// <summary>
//            /// Parses a string to EntrySignalType
//            /// </summary>
//            public static EntrySignalType Parse(string value)
//            {
//                if (string.IsNullOrWhiteSpace(value))
//                    return EntrySignalType.Breakout;

//                var normalized = value.ToLowerInvariant().Trim().Replace(" ", "");

//                return normalized switch
//                {
//                    "breakout" or "break" => EntrySignalType.Breakout,
//                    "pullback" or "pull" => EntrySignalType.Pullback,
//                    "reversal" or "rev" => EntrySignalType.Reversal,
//                    "continuation" or "cont" => EntrySignalType.Continuation,
//                    "supportbounce" or "support" => EntrySignalType.SupportBounce,
//                    "resistancebreak" or "resistance" => EntrySignalType.ResistanceBreak,
//                    "patterncompletion" or "pattern" => EntrySignalType.PatternCompletion,
//                    "macross" or "macrossover" => EntrySignalType.MaCross,
//                    "divergence" or "div" => EntrySignalType.Divergence,
//                    "oversold" => EntrySignalType.Oversold,
//                    "overbought" => EntrySignalType.Overbought,
//                    "retest" => EntrySignalType.Retest,
//                    "volumespike" or "volume" => EntrySignalType.VolumeSpike,
//                    "newscatalyst" or "news" => EntrySignalType.NewsCatalyst,
//                    "openingrangebreak" or "orb" => EntrySignalType.OpeningRangeBreak,
//                    "gapfill" or "gap" => EntrySignalType.GapFill,
//                    "fibonaccilevel" or "fib" => EntrySignalType.FibonacciLevel,
//                    "psychologicallevel" or "psych" => EntrySignalType.PsychologicalLevel,
//                    "sessiontransition" or "session" => EntrySignalType.SessionTransition,
//                    "momentum" or "mom" => EntrySignalType.Momentum,
//                    _ => Enum.TryParse<EntrySignalType>(value, true, out var result) ? result : EntrySignalType.Breakout
//                };
//            }
//        }
//    }