using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.DataAccess.Postgres.Repositories;
public interface IForecastServices
{
    Task<string> GetForecasts();
    Task<string> GetForecastsByDate(DateTime startDate, DateTime endDate);
    Task<string> GetForecastsByCountries(params string[] countries);
    Task<string> GetForecastsByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries);
    Task<string> GetForecastsByIndicator(params string[] indicators);
    Task<Forecast> Create(List<Forecast> forecasts);
}