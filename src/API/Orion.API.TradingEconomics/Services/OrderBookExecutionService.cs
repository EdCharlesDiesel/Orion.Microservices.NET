using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Services
{
    public class OrderBookExecutionService
    {
        public ExecutionOrder Execute(OrderBook book,string direction,decimal size)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction is required.", nameof(direction));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero.");

            direction = direction.Trim().ToUpperInvariant();

            var levels = direction switch
            {
                "LONG" => book.Asks,
                "BUY" => book.Asks,

                "SHORT" => book.Bids,
                "SELL" => book.Bids,

                _ => throw new ArgumentException(
                    "Direction must be LONG/BUY or SHORT/SELL.",
                    nameof(direction))
            };

            if (levels == null || levels.Count == 0)
            {
                return new ExecutionOrder
                {
                    Pair = book.Pair,
                    Direction = direction,
                    RequestedSize = size,
                    FilledSize = 0,
                    ExecutedPrice = 0,
                    Timestamp = DateTime.UtcNow
                };
            }

            decimal remaining = size;
            decimal totalCost = 0;
            decimal filled = 0;

            foreach (var level in levels)
            {
                if (remaining <= 0)
                    break;

                if (level.Volume <= 0)
                    continue;

                var take = Math.Min(remaining, (decimal)level.Volume);

                totalCost += take * level.Price;
                filled += take;
                remaining -= take;
            }

            var avgPrice = filled > 0 ? totalCost / filled : 0;

            return new ExecutionOrder
            {
                Pair = book.Pair,
                Direction = direction,
                RequestedSize = size,
                FilledSize = filled,
                ExecutedPrice = avgPrice,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
