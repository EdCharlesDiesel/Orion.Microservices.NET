using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Interfaces
{
    public interface IOrderBookProvider
    {
        Task<OrderBook> GetOrderBookAsync(string pair);
    }

    public interface ILatencyModel
    {
        Task<double> SimulateLatencyMsAsync();
    }

    public interface INewsEventService
    {
        Task<bool> IsHighImpactEventAsync(DateTime time);
    }
}
