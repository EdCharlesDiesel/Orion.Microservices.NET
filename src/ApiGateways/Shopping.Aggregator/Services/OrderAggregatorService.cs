using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Orion.Shopping.Aggregator.Extensions;
using Orion.Shopping.Aggregator.Models;

namespace Orion.Shopping.Aggregator.Services
{
    public class OrderAggregatorService(HttpClient client) : IOrderAggregatorService
    {
        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Order/{userName}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
