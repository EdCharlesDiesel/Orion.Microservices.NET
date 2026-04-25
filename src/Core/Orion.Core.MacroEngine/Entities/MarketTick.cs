using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class MarketTick
    {
        public string Pair { get; set; } = default!;
        public double Bid { get; set; }
        public double Ask { get; set; }
        public DateTime Time { get; set; }
    }
}
