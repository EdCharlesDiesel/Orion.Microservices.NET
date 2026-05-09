using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Models
{
    public class EconomicIndicator
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = default!;
        public string Indicator { get; set; } = default!;
        public DateTime Date { get; set; }

        public double? Value { get; set; }
        public double? Previous { get; set; }
        public double? Forecast { get; set; }

        public string Frequency { get; set; } = "Monthly";
    }
}
