using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Runs a real candle replay backtest and evaluates exits against open positions.
    /// </summary>
    public sealed class RealBacktestEngine : IRealBacktestEngine
    {
        private readonly IMarketReplayEngine _marketReplayEngine;
        private readonly ExitEngine _exitEngine;

        public RealBacktestEngine(
            IMarketReplayEngine marketReplayEngine,
            ExitEngine exitEngine)
        {
            _marketReplayEngine = marketReplayEngine ?? throw new ArgumentNullException(nameof(marketReplayEngine));
            _exitEngine = exitEngine ?? throw new ArgumentNullException(nameof(exitEngine));
        }

        /// <inheritdoc />
        public async Task<List<TradeResult>> RunAsync(
            List<Candle> candles,
            List<PortfolioPosition> positions,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(candles);
            ArgumentNullException.ThrowIfNull(positions);

            if (candles.Count == 0 || positions.Count == 0)
                return [];

            var openPositions = new List<OpenPosition>();
            var trades = new List<TradeResult>();

            await foreach (var candle in _marketReplayEngine.ReplayAsync(candles, cancellationToken: cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (openPositions.Count == 0)
                {
                    foreach (var position in positions.Where(x => x.PositionSize > 0m))
                    {
                        var atr = candle.High - candle.Low;

                        if (atr <= 0m)
                            continue;

                        var direction = position.Direction.Trim().ToUpperInvariant();

                        var (stopLoss, takeProfit) = RiskModel.Calculate(
                            candle.Close,
                            atr,
                            direction);

                        openPositions.Add(new OpenPosition
                        {
                            Pair = position.Pair.Trim().ToUpperInvariant(),
                            Direction = direction,
                            EntryPrice = candle.Close,
                            Size = position.PositionSize,
                            StopLoss = stopLoss,
                            TakeProfit = takeProfit
                        });
                    }
                }

                foreach (var position in openPositions.Where(x => !x.IsClosed))
                {
                    if (!_exitEngine.ShouldExit(position, candle, out var exitPrice))
                        continue;

                    position.IsClosed = true;

                    var pnl = (exitPrice - position.EntryPrice)
                              * position.Size
                              * (position.Direction == "LONG" ? 1m : -1m);

                    trades.Add(new TradeResult
                    {
                        Pair = position.Pair,
                        EntryTime = candle.Time,
                        ExitTime = candle.Time,
                        EntryPrice = position.EntryPrice,
                        ExitPrice = exitPrice,
                        PositionSize = position.Size,
                        PnL = Math.Round(pnl, 2)
                    });
                }
            }

            return trades;
        }
    }
}