using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public class BacktestEngine(IMediator mediator, AdvancedExecutionEngine execution)
    {
        public async Task<List<TradeResult>> RunAsync(DateTime start, DateTime end, decimal capital)
        {
            var trades = new List<TradeResult>();
            var currentCapital = capital;

            for (var date = start; date <= end; date = date.AddDays(1))
            {
                // 1. Generate portfolio
                var portfolio = await mediator.Send(new BuildPortfolioCommand(currentCapital));

                foreach (var position in portfolio)
                {
                    // 2. Execute trade
                    var exec = await execution.ExecuteAsync(
                        position.Pair,
                        position.Direction,
                        position.PositionSize);

                    // 3. Simulate exit (next period for now)
                    var exitPrice = exec.Order.ExecutedPrice * (1 + RandomReturn());

                    var pnl = (exitPrice - exec.Order.ExecutedPrice)
                              * exec.Order.FilledSize
                              * (position.Direction == "LONG" ? 1 : -1);

                    trades.Add(new TradeResult
                    {
                        Pair = position.Pair,
                        EntryTime = date,
                        ExitTime = date.AddDays(1),
                        EntryPrice = exec.Order.ExecutedPrice,
                        ExitPrice = exitPrice,
                        PositionSize = position.PositionSize,
                        PnL = pnl
                    });

                    currentCapital += pnl;
                }
            }

            return trades;
        }

        private decimal RandomReturn()
        {
            var rand = new Random();
            return (decimal)((rand.NextDouble() - 0.5) * 0.01); // ±0.5%
        }
    }
}
