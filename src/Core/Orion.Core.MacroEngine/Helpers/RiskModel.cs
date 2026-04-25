using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public static class RiskModel
    {
        public static (double sl, double tp) Calculate(double entry, double atr, string direction)
        {
            var slDistance = 1.5 * atr;
            var tpDistance = 3.0 * atr;

            if (direction == "LONG")
            {
                return (entry - slDistance, entry + tpDistance);
            }
            else
            {
                return (entry + slDistance, entry - tpDistance);
            }
        }
    }
}
