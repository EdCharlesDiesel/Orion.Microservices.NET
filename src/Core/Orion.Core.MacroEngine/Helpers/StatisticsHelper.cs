using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public static class StatisticsHelper
    {
        public static double ZScore(IEnumerable<double> values, double current)
        {
            var list = values.ToList();
            var mean = list.Average();
            var std = Math.Sqrt(list.Sum(v => Math.Pow(v - mean, 2)) / list.Count);

            return std == 0 ? 0 : (current - mean) / std;
        }
    }
}
