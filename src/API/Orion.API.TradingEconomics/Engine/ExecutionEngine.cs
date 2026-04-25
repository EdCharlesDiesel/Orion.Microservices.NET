using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class ExecutionEngine(IMarketDataFeed market, IExecutionCostModel cost)
    {
        public async Task<ExecutionOrder> ExecuteAsync(string pair, string direction, decimal size, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction is required.", nameof(direction));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero.");

            var normalizedDirection = direction.Trim().ToUpperInvariant();

            if (normalizedDirection is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(direction));

            var tick = await market.GetLatestTickAsync(pair, cancellationToken);

            if (tick == null)
                throw new InvalidOperationException($"No market tick returned for {pair}.");

            if (tick.Bid <= 0 || tick.Ask <= 0)
                throw new InvalidOperationException($"Invalid bid/ask for {pair}.");

            if (tick.Ask < tick.Bid)
                throw new InvalidOperationException($"Invalid market spread for {pair}. Ask is below bid.");

            var mid = (tick.Bid + tick.Ask) / 2m;

            var spread = cost.EstimateSpread(pair, tick.Bid, tick.Ask);
            var slippage = cost.EstimateSlippage(pair, size);

            var executedPrice = normalizedDirection == "LONG"
                ? tick.Ask + slippage
                : tick.Bid - slippage;

            return new ExecutionOrder
            {
                Pair = pair,
                Direction = normalizedDirection,
                RequestedSize = size,
                FilledSize = size,
                RequestedPrice = mid,
                ExecutedPrice = executedPrice,
                SpreadCost = spread,
                SlippageCost = slippage,
                Timestamp = DateTime.UtcNow
            };
        }

        public ExecutionOrder Execute(OrderBook orderBook, string signalDirection, decimal sizePositionSize)
        {
            if (orderBook == null)
                throw new ArgumentNullException(nameof(orderBook));

            if (string.IsNullOrWhiteSpace(signalDirection))
                throw new ArgumentException("Signal direction is required.", nameof(signalDirection));

            if (sizePositionSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(sizePositionSize), "Size must be greater than zero.");

            var direction = signalDirection.Trim().ToUpperInvariant();

            if (direction is not "LONG" and not "SHORT")
                throw new ArgumentException("Direction must be LONG or SHORT.", nameof(signalDirection));

            var levels = direction == "LONG"
                ? orderBook.Asks?.OrderBy(x => x.Price).ToList()
                : orderBook.Bids?.OrderByDescending(x => x.Price).ToList();

            if (levels == null || levels.Count == 0)
                throw new InvalidOperationException($"Order book has no liquidity for {direction}.");

            var remaining = sizePositionSize;
            decimal filled = 0m;
            decimal totalCost = 0m;

            foreach (var level in levels)
            {
                if (remaining <= 0)
                    break;

                if (level.Price <= 0 || level.Volume <= 0)
                    continue;

                var take = Math.Min(remaining, level.Volume);

                totalCost += take * level.Price;
                filled += take;
                remaining -= take;
            }

            if (filled <= 0)
                throw new InvalidOperationException("Order could not be filled.");

            var averageExecutedPrice = totalCost / filled;

            var bestBid = orderBook.Bids?
                .Where(x => x.Price > 0)
                .OrderByDescending(x => x.Price)
                .FirstOrDefault();

            var bestAsk = orderBook.Asks?
                .Where(x => x.Price > 0)
                .OrderBy(x => x.Price)
                .FirstOrDefault();

            var requestedPrice = bestBid != null && bestAsk != null
                ? (bestBid.Price + bestAsk.Price) / 2m
                : averageExecutedPrice;

            return new ExecutionOrder
            {
                Pair = orderBook.Pair,
                Direction = direction,
                RequestedSize = sizePositionSize,
                FilledSize = filled,
                RequestedPrice = requestedPrice,
                ExecutedPrice = averageExecutedPrice,
                SpreadCost = bestBid != null && bestAsk != null
                    ? bestAsk.Price - bestBid.Price
                    : 0m,
                SlippageCost = Math.Abs(averageExecutedPrice - requestedPrice),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}