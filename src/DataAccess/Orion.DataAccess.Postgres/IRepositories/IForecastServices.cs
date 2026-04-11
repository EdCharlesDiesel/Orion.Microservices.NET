using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orion.Domain.IRepositories;
public interface IForecastServices
{
    Task<string> GetForecasts();
    Task<string> GetForecastsByDate(DateTime startDate, DateTime endDate);
    Task<string> GetForecastsByCountries(params string[] countries);
    Task<string> GetForecastsByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries);
    Task<string> GetForecastsByIndicator(params string[] indicators);
    Task<Forecast> Create(List<Forecast>? forecasts);
}

public class Forecast
{
    public string CreateBy { get; set; }
    public string Country { get; set; }
}