using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Helpers
{
    public class TradingEconomicsClient(HttpClient http) : ITradingEconomicsClient
    {
        public async Task<IEnumerable<EconomicIndicator>> GetIndicatorsAsync(string country)
        {
            try
            {
                var url = $"https://api.tradingeconomics.com/calendar/country/{country}?c=YOUR_KEY";

                var response = await http.GetFromJsonAsync<TradingEconomicsResponse<EconomicIndicator>>(url);

                return response?.Data.Select(x => new EconomicIndicator
                {
                    Id = Guid.NewGuid(),
                    Country = x.Country,
                    Indicator = x.Event,
                    Date = x.Date,
                    Value = x.Actual,
                    Previous = x.Previous,
                    Forecast = x.Forecast,
                    Frequency = "Monthly"
                }) ?? [];
            }
            catch (Exception)
            {
                // In production: log + fallback cache
                return [];
            }
        }
    }
}
