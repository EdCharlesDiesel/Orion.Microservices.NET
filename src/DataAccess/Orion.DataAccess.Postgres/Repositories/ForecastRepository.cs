using Orion.DataAccess.Postgres.Data;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ForecastRepository(OrionDbContext context) : IForecastServices
{
    // public async Task<string> GetForecasts()
    // {
    //     return await HttpRequesterClass.HttpRequester("/forecasts");
    // }
    //
    // public async Task<string> GetForecastsByDate(DateTime startDate, DateTime endDate)
    // {
    //     if (startDate == default || endDate == default)
    //         return "Invalid date range";
    //
    //     string textStartDate = startDate.ToString("yyyy-MM-dd");
    //     string textEndDate = endDate.ToString("yyyy-MM-dd");
    //
    //     return await HttpRequesterClass.HttpRequester($"/forecast/country/all/{textStartDate}/{textEndDate}");
    // }
    //
    // public async Task<string> GetForecastsByCountries(params string[] countries)
    // {
    //     if (countries.Any(string.IsNullOrWhiteSpace))
    //         return "Invalid country names";
    //
    //     return await HttpRequesterClass.HttpRequester($"/forecast/country/{string.Join(",", countries)}");
    // }
    //
    // public async Task<string> GetForecastsByCountriesAndDates(DateTime startDate, DateTime endDate,
    //     params string[] countries)
    // {
    //     if (startDate == default || endDate == default || countries.Any(string.IsNullOrWhiteSpace))
    //         return "Invalid input";
    //
    //     string textStartDate = startDate.ToString("yyyy-MM-dd");
    //     string textEndDate = endDate.ToString("yyyy-MM-dd");
    //
    //     return await HttpRequesterClass.HttpRequester(
    //         $"/forecast/country/{string.Join(",", countries)}/{textStartDate}/{textEndDate}");
    // }
    //
    // public async Task<string> GetForecastsByIndicator(params string[] indicators)
    // {
    //     if (indicators.Any(string.IsNullOrWhiteSpace))
    //         return "Invalid indicator names";
    //
    //     return await HttpRequesterClass.HttpRequester($"/forecast/indicator/{string.Join(",", indicators)}");
    // }
    //
    // public async Task<Forecast> Create(List<Forecast> forecastEvents)
    // {
    //     if (forecastEvents == null || !forecastEvents.Any())
    //         throw new ArgumentException("Event list cannot be null or empty.");
    //
    //     await context.Forecast.AddRangeAsync(forecastEvents);
    //     await context.SaveChangesAsync();
    //
    //     // Return the first created event (or you can change this to return the list)
    //     return forecastEvents.First();
    // }


    public async Task<string> GetForecasts()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetForecastsByDate(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetForecastsByCountries(params string[] countries)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetForecastsByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetForecastsByIndicator(params string[] indicators)
    {
        throw new NotImplementedException();
    }

    public async Task<Entities.TradingEconomics.Forecast> Create(List<Entities.TradingEconomics.Forecast> forecasts)
    {
        throw new NotImplementedException();
    }

    public async Task<Forecast> Create(List<Forecast>? forecasts)
    {
        throw new NotImplementedException();
    }
}