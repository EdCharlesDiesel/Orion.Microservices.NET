using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Service
{
    public class OrderBookExecutionService
    {
        public ExecutionOrder Execute(
            OrderBook book,
            string direction,
            double size)
        {
            var remaining = size;
            double totalCost = 0;
            double filled = 0;

            var levels = direction == "LONG" ? book.Asks : book.Bids;

            foreach (var level in levels)
            {
                if (remaining <= 0) break;

                var take = Math.Min(remaining, level.Volume);

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
