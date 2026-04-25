using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class OpenPosition
    {
        public string Pair { get; set; } = default!;
        public string Direction { get; set; } = default!;

        public double EntryPrice { get; set; }
        public double Size { get; set; }

        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }

        public bool IsClosed { get; set; }
    }
}
