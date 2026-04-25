using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public static class ExecutionTiming
    {
        public static bool IsGoodLiquidityWindow(DateTime utcNow)
        {
            var hour = utcNow.Hour;

            // London + NY overlap
            return hour >= 12 && hour <= 16;
        }
    }
}
