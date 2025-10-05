using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Aggregator.Models;

namespace Orion.Aggregator.Services
{
    public interface IOrderAggregatorService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
