//using Orion.WebApps.AnalysisDashboard.Models;

//namespace Orion.WebApps.AnalysisDashboard.Services
//{
//    public class BacktestService
//    {
//        private const int MinBarsForEma = 30;
//        private const int MaxLookAheadBars = 20;
//        private const decimal PipToPriceFactor = 0.0001m;

//        /// <summary>
//        /// Runs an EMA crossover backtest on the provided price data.
//        /// Generates a list of trades based on 9/21 EMA cross signals.
//        /// </summary>
//        /// <param name="data">Raw price data</param>
//        /// <param name="slPips">Stop loss in pips</param>
//        /// <param name="tpPips">Take profit in pips</param>
//        /// <returns>List of executed trades</returns>
//        public List<Trade> RunEmaCrossBacktest(List<PriceData> data, int slPips, int tpPips)
//        {
//            var trades = new List<Trade>();            
//            if (data == null || data.Count == 0)
//                return trades;

//            if (slPips <= 0 || tpPips <= 0)
//                throw new ArgumentException("Stop loss and take profit must be positive values");
            
//            var m15Data = ResampleToM15(data);
            
//            if (m15Data.Count < MinBarsForEma)
//                return trades;
            
//            var sl = slPips * PipToPriceFactor;
//            var tp = tpPips * PipToPriceFactor;
            
//            for (int i = 1; i < m15Data.Count; i++)
//            {
//                var prev = m15Data[i - 1];
//                var curr = m15Data[i];
                
//                if (!prev.Ema9.HasValue || !prev.Ema21.HasValue ||
//                    !curr.Ema9.HasValue || !curr.Ema21.HasValue)
//                    continue;
                
//                if (prev.Ema9 <= prev.Ema21 && curr.Ema9 > curr.Ema21)
//                {
//                    var trade = ExecuteLongTrade(m15Data, i, curr.Close, sl, tp, slPips, tpPips);
//                    if (trade != null)
//                        trades.Add(trade);
//                }
                
//                else if (prev.Ema9 >= prev.Ema21 && curr.Ema9 < curr.Ema21)
//                {
//                    var trade = ExecuteShortTrade(m15Data, i, curr.Close, sl, tp, slPips, tpPips);
//                    if (trade != null)
//                        trades.Add(trade);
//                }
//            }

//            return trades;
//        }

//        /// <summary>
//        /// Resamples minute-based price data to 15-minute intervals
//        /// </summary>
//        /// <param name="minuteData">Raw minute-by-minute price data</param>
//        /// <returns>15-minute resampled price data</returns>
//        private List<PriceData> ResampleToM15(List<PriceData> minuteData)
//        {
//            if (minuteData == null || minuteData.Count == 0)
//                return new List<PriceData>();

//            return minuteData
//                .GroupBy(d => new DateTime(d.DateTime.Year, d.DateTime.Month, d.DateTime.Day,
//                    d.DateTime.Hour, (d.DateTime.Minute / 15) * 15, 0))
//                .Select(g => new PriceData
//                {
//                    DateTime = g.Key,
//                    Open = g.First().Open,
//                    High = g.Max(x => x.High),
//                    Low = g.Min(x => x.Low),
//                    Close = g.Last().Close,
//                    Volume = g.Sum(x => x.Volume)
//                })
//                .OrderBy(r => r.DateTime)
//                .ToList();
//        }

//        /// <summary>
//        /// Executes a long trade and monitors for take profit or stop loss
//        /// </summary>
//        /// <param name="data">Price data array</param>
//        /// <param name="startIndex">Index where trade was triggered</param>
//        /// <param name="entry">Entry price</param>
//        /// <param name="sl">Stop loss price level</param>
//        /// <param name="tp">Take profit price level</param>
//        /// <param name="slPips">Stop loss in pips (for PnL calculation)</param>
//        /// <param name="tpPips">Take profit in pips (for PnL calculation)</param>
//        /// <returns>Trade result or null if no SL/TP hit within lookahead period</returns>
//        private Trade? ExecuteLongTrade(
//            List<PriceData> data,
//            int startIndex, 
//            decimal entry,
//            decimal stopLoss, 
//            decimal takeProfit, 
//            int stopLossPips,
//            int takeProfitPips)
//        {
//            var lookAhead = Math.Min(startIndex + MaxLookAheadBars, data.Count);

//            for (int j = startIndex + 1; j < lookAhead; j++)
//            {                
//                if (data[j].High >= entry + takeProfit)
//                {
//                    return new Trade
//                    {
//                        DateTime = data[j].DateTime,
//                        Direction = "LONG",
//                        Entry = entry,
//                        Exit = entry + takeProfit,
//                        Result = "TP",
//                        ProfitAndLoss = takeProfitPips
//                    };
//                }
                
//                if (data[j].Low <= entry - stopLoss)
//                {
//                    return new Trade
//                    {
//                        DateTime = data[j].DateTime,
//                        Direction = "LONG",
//                        Entry = entry,
//                        Exit = entry - stopLoss,
//                        Result = "SL",
//                        ProfitAndLoss = -stopLossPips
//                    };
//                }
//            }

//            return null; 
//        }

//        /// <summary>
//        /// Executes a short trade and monitors for take profit or stop loss
//        /// </summary>
//        /// <param name="data">Price data array</param>
//        /// <param name="startIndex">Index where trade was triggered</param>
//        /// <param name="entry">Entry price</param>
//        /// <param name="sl">Stop loss price level</param>
//        /// <param name="tp">Take profit price level</param>
//        /// <param name="slPips">Stop loss in pips (for PnL calculation)</param>
//        /// <param name="tpPips">Take profit in pips (for PnL calculation)</param>
//        /// <returns>Trade result or null if no SL/TP hit within lookahead period</returns>
//        private Trade? ExecuteShortTrade(
//            List<PriceData> data, 
//            int startIndex, 
//            decimal entry,
//            decimal stopLoss, 
//            decimal takeProfit, 
//            int stopLossPips, 
//            int takeProfitPips)
//        {
//            var lookAhead = Math.Min(startIndex + MaxLookAheadBars, data.Count);

//            for (int j = startIndex + 1; j < lookAhead; j++)
//            {                
//                if (data[j].Low <= entry - takeProfit)
//                {
//                    return new Trade
//                    {
//                        DateTime = data[j].DateTime,
//                        Direction = "SHORT",
//                        Entry = entry,
//                        Exit = entry - takeProfit,
//                        Result = "TP",
//                        ProfitAndLoss = takeProfitPips
//                    };
//                }
                
//                if (data[j].High >= entry + stopLoss)
//                {
//                    return new Trade
//                    {
//                        DateTime = data[j].DateTime,
//                        Direction = "SHORT",
//                        Entry = entry,
//                        Exit = entry + stopLoss,
//                        Result = "SL",
//                        ProfitAndLoss = -stopLossPips
//                    };
//                }
//            }

//            return null; 
//        }
//    }
//}