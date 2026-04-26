using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Engine.Interfaces.Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Handles trade execution using live market data or order book depth.
    /// </summary>
    public sealed class ExecutionEngine : IExecutionEngine
    {
        private readonly IMarketDataEngine _market;
        private readonly IExecutionCostModel _cost;

        public ExecutionEngine(IMarketDataEngine market, IExecutionCostModel cost)
        {
            _market = market ?? throw new ArgumentNullException(nameof(market));
            _cost = cost ?? throw new ArgumentNullException(nameof(cost));
        }

        /// <inheritdoc />
        public async Task<ExecutionOrder> ExecuteAsync(string pair, string direction, decimal size, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction is required.", nameof(direction));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            var dir = NormalizeDirection(direction);

            var tick = await _market.GetLatestTickAsync(pair, cancellationToken);

            if (tick == null)
                throw new InvalidOperationException($"No market tick returned for {pair}.");

            if (tick.Bid <= 0 || tick.Ask <= 0 || tick.Ask < tick.Bid)
                throw new InvalidOperationException($"Invalid bid/ask for {pair}.");

            var mid = (tick.Bid + tick.Ask) / 2m;

            var spread = _cost.EstimateSpread(pair, tick.Bid, tick.Ask);
            var slippage = _cost.EstimateSlippage(pair, size);

            var executedPrice = dir == "LONG"
                ? tick.Ask + slippage
                : tick.Bid - slippage;

            return new ExecutionOrder
            {
                Pair = pair,
                Direction = dir,
                RequestedSize = size,
                FilledSize = size,
                RequestedPrice = mid,
                ExecutedPrice = executedPrice,
                SpreadCost = spread,
                SlippageCost = slippage,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <inheritdoc />
        public ExecutionOrder Execute(
            OrderBook orderBook,
            string direction,
            decimal size)
        {
            ArgumentNullException.ThrowIfNull(orderBook);

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException(nameof(direction));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            var dir = NormalizeDirection(direction);

            var levels = dir == "LONG"
                ? orderBook.Asks?.OrderBy(x => x.Price).ToList()
                : orderBook.Bids?.OrderByDescending(x => x.Price).ToList();

            if (levels == null || levels.Count == 0)
                throw new InvalidOperationException("No liquidity.");

            decimal filled = 0;
            decimal cost = 0;
            var remaining = size;

            foreach (var l in levels)
            {
                if (remaining <= 0) break;
                if (l.Price <= 0 || l.Volume <= 0) continue;

                var take = Math.Min(remaining, l.Volume);

                cost += take * l.Price;
                filled += take;
                remaining -= take;
            }

            if (filled <= 0)
                throw new InvalidOperationException("Order not filled.");

            var avg = cost / filled;

            return new ExecutionOrder
            {
                Pair = orderBook.Pair,
                Direction = dir,
                RequestedSize = size,
                FilledSize = filled,
                RequestedPrice = avg,
                ExecutedPrice = avg,
                Timestamp = DateTime.UtcNow
            };
        }

        private static string NormalizeDirection(string d)
        {
            var dir = d.Trim().ToUpperInvariant();

            if (dir is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.");

            return dir;
        }
    }
}