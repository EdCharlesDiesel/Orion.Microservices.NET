using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Models
{
    public class NormalizedIndicator
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = default!;
        public string Indicator { get; set; } = default!;
        public DateTime Date { get; set; }

        public double Value { get; set; }
        public double YoY { get; set; }
        public double MoM { get; set; }
        public double ZScore { get; set; }
        public double Surprise { get; set; }
    }
}
