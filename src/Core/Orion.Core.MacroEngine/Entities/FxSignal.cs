using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class FxSignal
    {
        public string BaseCurrency { get; set; } = default!;
        public string QuoteCurrency { get; set; } = default!;
        public string Pair => $"{BaseCurrency}/{QuoteCurrency}";

        public double BaseScore { get; set; }
        public double QuoteScore { get; set; }

        public double SignalStrength { get; set; } // difference
        public string Direction { get; set; } = default!; // LONG / SHORT

        public double Confidence { get; set; }
    }
}
