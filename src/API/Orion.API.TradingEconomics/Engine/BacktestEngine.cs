using MediatR;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Executes a simple backtest by generating portfolios, executing trades,
/// and simulating exits over a date range.
/// </summary>
public sealed class BacktestEngine(
    IMediator mediator,
    AdvancedExecutionEngine execution,
    Random? random = null)
    : IBacktestEngine
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly AdvancedExecutionEngine _execution = execution ?? throw new ArgumentNullException(nameof(execution));
    private readonly Random _random = random ?? new Random();

    /// <summary>
    /// Runs a backtest between two dates using the provided capital.
    /// </summary>
    public async Task<List<TradeResult>> RunAsync(
        DateTime start,
        DateTime end,
        decimal capital)
    {
        if (end < start)
            throw new ArgumentException("End date cannot be before start date.", nameof(end));

        if (capital <= 0)
            throw new ArgumentException("Capital must be greater than zero.", nameof(capital));

        var trades = new List<TradeResult>();
        var currentCapital = capital;

        for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            var portfolio = await _mediator.Send(new BuildPortfolioCommand(currentCapital))
                             ?? new List<PortfolioPosition>();

            foreach (var position in portfolio)
            {
                var exec = await _execution.ExecuteAsync(
                    position.Pair,
                    position.Direction,
                    position.PositionSize);

                if (exec?.Order == null || exec.Order.FilledSize <= 0)
                    continue;

                var entryPrice = exec.Order.ExecutedPrice;
                var exitPrice = entryPrice * (1 + RandomReturn());

                var directionMultiplier = string.Equals(position.Direction, "LONG", StringComparison.OrdinalIgnoreCase) ? 1 : -1;

                var pnl = (exitPrice - entryPrice)
                          * exec.Order.FilledSize
                          * directionMultiplier;

                trades.Add(new TradeResult
                {
                    Pair = position.Pair,
                    EntryTime = date,
                    ExitTime = date.AddDays(1),
                    EntryPrice = entryPrice,
                    ExitPrice = exitPrice,
                    PositionSize = exec.Order.FilledSize,
                    PnL = pnl
                });

                currentCapital += pnl;
            }
        }

        return trades;
    }

    private decimal RandomReturn()
    {
        return (decimal)((_random.NextDouble() - 0.5) * 0.01); // ±0.5%
    }
}