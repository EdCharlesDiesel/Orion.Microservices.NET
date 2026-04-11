using System.Threading.Tasks;
using Orion.Shopping.Aggregator.Models;

namespace Orion.Shopping.Aggregator.Services
{
    public interface IBasketAggregatorService
    {
        Task<BasketModel> GetBasket(string userName);                
    }
}
