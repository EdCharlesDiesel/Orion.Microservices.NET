using Orion.Core.MacroEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Service
{
    public class DummyVolatilityService : IVolatilityService
    {
        public Task<double> GetVolatilityAsync(string pair)
        {
            // Replace with real ATR / historical volatility
            var random = new Random();
            return Task.FromResult(0.5 + random.NextDouble());
        }
    }
}
