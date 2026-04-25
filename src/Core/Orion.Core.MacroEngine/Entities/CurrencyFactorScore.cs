using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Models
{
    public class CurrencyFactorScore
    {
        public string Currency { get; set; } = default!;
        public DateTime Date { get; set; }

        public double Carry { get; set; }
        public double Growth { get; set; }
        public double Inflation { get; set; }
        public double Risk { get; set; }

        public double TotalScore { get; set; }
    }
}
