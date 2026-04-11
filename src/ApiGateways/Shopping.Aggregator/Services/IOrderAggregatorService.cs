using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Shopping.Aggregator.Models;

namespace Orion.Shopping.Aggregator.Services
{
    public interface IOrderAggregatorService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
