using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public static class PerformanceMetrics
    {
        public static double SharpeRatio(List<double> returns)
        {
            var avg = returns.Average();
            var std = Math.Sqrt(returns.Sum(r => Math.Pow(r - avg, 2)) / returns.Count);

            return std == 0 ? 0 : avg / std * Math.Sqrt(252);
        }

        public static double MaxDrawdown(List<double> equity)
        {
            double peak = equity[0];
            double maxDd = 0;

            foreach (var e in equity)
            {
                if (e > peak) peak = e;

                var dd = (peak - e) / peak;
                if (dd > maxDd) maxDd = dd;
            }

            return maxDd;
        }
    }
}
