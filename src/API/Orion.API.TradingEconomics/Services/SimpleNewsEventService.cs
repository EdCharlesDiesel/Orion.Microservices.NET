using Orion.API.TradingEconomics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.API.TradingEconomics.Services
{
    public class SimpleNewsEventService : INewsEventService
    {
        public Task<bool> IsHighImpactEventAsync(DateTime time)
        {
            // Placeholder logic:
            // You should replace with TradingEconomics calendar

            var hour = time.Hour;

            // Simulate common macro release windows
            var isEvent = hour == 12 || hour == 14;

            return Task.FromResult(isEvent);
        }
    }
}
