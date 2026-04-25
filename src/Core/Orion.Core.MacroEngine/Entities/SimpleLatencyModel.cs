using Orion.Core.MacroEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class SimpleLatencyModel : ILatencyModel
    {
        public Task<double> SimulateLatencyMsAsync()
        {
            var random = new Random();

            // Simulate 10–150ms latency
            var latency = 10 + random.NextDouble() * 140;

            return Task.FromResult(latency);
        }
    }
}
