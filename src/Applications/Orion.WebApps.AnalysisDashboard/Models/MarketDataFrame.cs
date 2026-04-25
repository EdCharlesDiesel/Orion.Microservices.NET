using Skender.Stock.Indicators;

namespace Orion.WebApps.AnalysisDashboard.Models
{
    /// <summary>
    /// Wraps an ordered collection of OHLCV quotes.
    /// MarketDataRow is replaced by Skender's Quote — no custom model needed.
    /// </summary>
    public class MarketDataFrame
    {        
        public static MarketDataFrame Empty => new();        
        private readonly List<Quote> _quotes;
        public bool IsEmpty => _quotes.Count == 0;
        public IReadOnlyList<Quote> Quotes => _quotes;
        public MarketDataFrame() => _quotes = new();

        /// <summary>
        /// Accepts any IQuote source — including your Candle class.
        /// Skender validates and sorts the quotes automatically.
        /// </summary>
        public MarketDataFrame(IEnumerable<IQuote> source)
        {            
            _quotes = source
                .OrderBy(q => q.Date)
                .Select(q => new Quote
                {
                    Date = q.Date,
                    Open = q.Open,
                    High = q.High,
                    Low = q.Low,
                    Close = q.Close,
                    Volume = q.Volume
                })
                .ToList();
        }
        
        /// <summary>
        /// Aggregates M1 bars into any higher timeframe.
        /// e.g. TimeSpan.FromMinutes(15) → M15
        ///      TimeSpan.FromHours(1)    → H1
        ///      TimeSpan.FromHours(4)    → H4
        /// </summary>
        public MarketDataFrame Resample(TimeSpan interval)
        {
            var resampled = _quotes
                .GroupBy(q => RoundDown(q.Date, interval))
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    var bars = g.ToList();
                    return new Quote
                    {
                        Date = g.Key,
                        Open = bars.First().Open,
                        High = bars.Max(b => b.High),
                        Low = bars.Min(b => b.Low),
                        Close = bars.Last().Close,
                        Volume = bars.Sum(b => b.Volume)
                    };
                });

            return new MarketDataFrame(resampled);
        }

        // ── Skender indicator pass-through ─────────────────────
        /// <summary>
        /// Exposes quotes directly to any Skender extension method.
        /// e.g. frame.AsIndicatorInput().GetSma(20)
        /// </summary>
        public IEnumerable<Quote> AsIndicatorInput() => _quotes;
        
        private static DateTime RoundDown(DateTime dt, TimeSpan interval)
        {
            long ticks = dt.Ticks / interval.Ticks;
            return new DateTime(ticks * interval.Ticks, dt.Kind);
        }
    }
}