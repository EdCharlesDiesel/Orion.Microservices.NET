using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class OrderBookLevel
    {
        public double Price { get; set; }
        public double Volume { get; set; }
    }

    public class OrderBook
    {
        public string Pair { get; set; } = default!;
        public List<OrderBookLevel> Bids { get; set; } = new();
        public List<OrderBookLevel> Asks { get; set; } = new();
        public DateTime Timestamp { get; set; }
    }

    public class ExecutionResult
    {
        public ExecutionOrder Order { get; set; } = default!;
        public bool PartialFill { get; set; }
        public double FillRatio { get; set; }
        public double LatencyMs { get; set; }
        public bool HighImpactEvent { get; set; }
    }
}
