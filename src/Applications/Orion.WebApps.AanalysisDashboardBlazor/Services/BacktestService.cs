using Orion.WebApps.AanalysisDashboardBlazor.Models;

namespace Orion.WebApps.AanalysisDashboardBlazor.Services
{
    public class BacktestService
    {
        public List<Trade> RunEmaCrossBacktest(List<PriceData> data, int slPips, int tpPips)
        {
            var trades = new List<Trade>();
            var m15Data = ResampleToM15(data);

            if (m15Data.Count < 30) return trades;

            var sl = slPips * 0.0001m;
            var tp = tpPips * 0.0001m;

            for (int i = 1; i < m15Data.Count; i++)
            {
                var prev = m15Data[i - 1];
                var curr = m15Data[i];

                if (!prev.Ema9.HasValue || !prev.Ema21.HasValue || !curr.Ema9.HasValue || !curr.Ema21.HasValue)
                    continue;

                // Bull cross
                if (prev.Ema9 <= prev.Ema21 && curr.Ema9 > curr.Ema21)
                {
                    var trade = ExecuteLongTrade(m15Data, i, curr.Close, sl, tpPips, tp);
                    if (trade != null) trades.Add(trade);
                }
                // Bear cross
                else if (prev.Ema9 >= prev.Ema21 && curr.Ema9 < curr.Ema21)
                {
                    var trade = ExecuteShortTrade(m15Data, i, curr.Close, sl, tpPips, tp);
                    if (trade != null) trades.Add(trade);
                }
            }

            return trades;
        }

        private List<PriceData> ResampleToM15(List<PriceData> minuteData)
        {
            return minuteData
                .GroupBy(d => new DateTime(d.DateTime.Year, d.DateTime.Month, d.DateTime.Day,
                    d.DateTime.Hour, (d.DateTime.Minute / 15) * 15, 0))
                .Select(g => new PriceData
                {
                    DateTime = g.Key,
                    Open = g.First().Open,
                    High = g.Max(x => x.High),
                    Low = g.Min(x => x.Low),
                    Close = g.Last().Close,
                    Volume = g.Sum(x => x.Volume)
                })
                .OrderBy(r => r.DateTime)
                .ToList();
        }

        private Trade? ExecuteLongTrade(List<PriceData> data, int startIndex, decimal entry,
            decimal sl, int tpPips, decimal tp)
        {
            var lookAhead = Math.Min(startIndex + 20, data.Count);

            for (int j = startIndex + 1; j < lookAhead; j++)
            {
                if (data[j].High >= entry + tp)
                {
                    return new Trade
                    {
                        DateTime = data[j].DateTime,
                        Direction = "LONG",
                        Entry = entry,
                        Result = "TP",
                        PnL = tpPips
                    };
                }
                if (data[j].Low <= entry - sl)
                {
                    return new Trade
                    {
                        DateTime = data[j].DateTime,
                        Direction = "LONG",
                        Entry = entry,
                        Result = "SL",
                        PnL = -slPips
                    };
                }
            }

            return null;
        }

        private Trade? ExecuteShortTrade(List<PriceData> data, int startIndex, decimal entry,
            decimal sl, int tpPips, decimal tp)
        {
            var lookAhead = Math.Min(startIndex + 20, data.Count);

            for (int j = startIndex + 1; j < lookAhead; j++)
            {
                if (data[j].Low <= entry - tp)
                {
                    return new Trade
                    {
                        DateTime = data[j].DateTime,
                        Direction = "SHORT",
                        Entry = entry,
                        Result = "TP",
                        PnL = tpPips
                    };
                }
                if (data[j].High >= entry + sl)
                {
                    return new Trade
                    {
                        DateTime = data[j].DateTime,
                        Direction = "SHORT",
                        Entry = entry,
                        Result = "SL",
                        PnL = -slPips
                    };
                }
            }

            return null;
        }
    }
}
