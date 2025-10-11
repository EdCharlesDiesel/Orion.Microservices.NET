using System;
using System.Net.Http;
using System.Threading.Tasks;
using Orion.Aggregator.Models;
using Orion.Aggregator.Extensions;

namespace Orion.Aggregator.Services
{
    public class BasketAggregatorService(HttpClient client) : IBasketAggregatorService
    {
        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }        
    }
}
