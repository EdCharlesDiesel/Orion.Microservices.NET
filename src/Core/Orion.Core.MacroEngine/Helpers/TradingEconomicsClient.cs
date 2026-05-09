using Orion.Core.MacroEngine.Interfaces;
using Orion.Core.MacroEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Helpers
{
    public class TradingEconomicsClient : ITradingEconomicsClient
    {
        private readonly HttpClient _http;

        public TradingEconomicsClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<EconomicIndicator>> GetIndicatorsAsync(string country)
        {
            var response = await _http.GetFromJsonAsync<List<TradingEconomicsResponse>>(
                $"https://api.tradingeconomics.com/calendar/country/{country}?c=YOUR_KEY");

            return response?.Select(x => new EconomicIndicator
            {
                Id = Guid.NewGuid(),
                Country = x.Country,
                Indicator = x.Event,
                Date = x.Date,
                Value = x.Actual,
                Previous = x.Previous,
                Forecast = x.Forecast
            }) ?? Enumerable.Empty<EconomicIndicator>();
        }
    }
}
