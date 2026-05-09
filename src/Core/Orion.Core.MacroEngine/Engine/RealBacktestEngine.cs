using Orion.Core.MacroEngine.BacktestEngine;
using Orion.Core.MacroEngine.Entities;
using Orion.Core.MacroEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Engine
{
    public class RealBacktestEngine
    {
        private readonly ExitEngine _exit = new();

        public async Task<List<TradeResult>> RunAsync(
            List<Candle> candles,
            List<PortfolioPosition> positions)
        {
            var replay = new MarketReplayEngine();
            var openPositions = new List<OpenPosition>();
            var trades = new List<TradeResult>();

            await foreach (var candle in replay.ReplayAsync(candles))
            {
                // 1. Open positions at first candle (simplification)
                if (!openPositions.Any())
                {
                    foreach (var p in positions)
                    {
                        var atr = (candle.High - candle.Low);

                        var (sl, tp) = RiskModel.Calculate(
                            candle.Close,
                            atr,
                            p.Direction);

                        openPositions.Add(new OpenPosition
                        {
                            Pair = p.Pair,
                            Direction = p.Direction,
                            EntryPrice = candle.Close,
                            Size = p.PositionSize,
                            StopLoss = sl,
                            TakeProfit = tp
                        });
                    }
                }

                // 2. Evaluate exits on every candle
                foreach (var pos in openPositions.Where(x => !x.IsClosed))
                {
                    if (_exit.ShouldExit(pos, candle, out var exitPrice))
                    {
                        pos.IsClosed = true;

                        var pnl = (exitPrice - pos.EntryPrice)
                                  * pos.Size
                                  * (pos.Direction == "LONG" ? 1 : -1);

                        trades.Add(new TradeResult
                        {
                            Pair = pos.Pair,
                            EntryTime = candle.Time,
                            ExitTime = candle.Time,
                            EntryPrice = pos.EntryPrice,
                            ExitPrice = exitPrice,
                            PositionSize = pos.Size,
                            PnL = pnl
                        });
                    }
                }
            }

            return trades;
        }
    }
}
