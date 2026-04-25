using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class PortfolioPosition
    {
        public string Pair { get; set; } = default!;
        public string BaseCurrency { get; set; } = default!;
        public string QuoteCurrency { get; set; } = default!;

        public string Direction { get; set; } = default!; // LONG / SHORT

        public double SignalStrength { get; set; }
        public double Confidence { get; set; }

        public double Volatility { get; set; } // e.g. ATR
        public double Weight { get; set; }     // normalized portfolio weight
        public double PositionSize { get; set; } // capital allocation
    }
}
