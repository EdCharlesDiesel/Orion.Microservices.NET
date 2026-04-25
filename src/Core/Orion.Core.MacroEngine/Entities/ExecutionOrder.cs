using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class ExecutionOrder
    {
        public string Pair { get; set; } = default!;
        public string Direction { get; set; } = default!;

        public double RequestedSize { get; set; }
        public double FilledSize { get; set; }

        public double RequestedPrice { get; set; }
        public double ExecutedPrice { get; set; }

        public double SpreadCost { get; set; }
        public double SlippageCost { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
