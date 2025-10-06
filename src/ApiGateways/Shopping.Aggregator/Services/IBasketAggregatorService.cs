using System.Threading.Tasks;
using Orion.Aggregator.Models;

namespace Orion.Aggregator.Services
{
    public interface IBasketAggregatorService
    {
        Task<BasketModel> GetBasket(string userName);                
    }
}
