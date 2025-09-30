using System.Net.Http.Headers;
using System.Text;
using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.DataAccess.Postgres.Repositories;
public class TradingEconomicsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "guest:guest";

    public TradingEconomicsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.tradingeconomics.com");
        var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(_apiKey));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedKey);
    }

    public async Task<string> GetLatestNewsAsync()
    {
        var response = await _httpClient.GetAsync("/news");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetNewsByCountryAsync(string[] countries)
    {
        string path = $"/news/country/{string.Join(",", countries)}";
        var response = await _httpClient.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetNewsByIndicatorAsync(string[] indicators)
    {
        string path = $"/news/indicator/{string.Join(",", indicators)}";
        var response = await _httpClient.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> GetIntradaySymbolAsync(string symbol)
    {
        var response = await _httpClient.GetAsync($"/markets/intraday/{symbol}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetIntradayDateHourAsync(string symbol, DateTime startDate)
    {
        var query = startDate.ToString("yyyy-MM-dd HH");
        var response = await _httpClient.GetAsync($"/markets/intraday/{symbol}?{query}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetIntradaySymbolDatesAsync(string symbol, DateTime startDate, DateTime endDate)
    {
        var start = startDate.ToString("yyyy-MM-dd");
        var end = endDate.ToString("yyyy-MM-dd");
        var response = await _httpClient.GetAsync($"/markets/intraday/{symbol}?{start}/{end}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetCalendarEvents()
    {
        throw new NotImplementedException();
    }

    public async Task Create(List<TradingEconomicsCalendar> calendarEvents)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCalendarEventsByDate(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCalendarEventsByCountries(string[] names)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCalendarEventsByCountriesAndDates(DateTime startDate, DateTime endDate, string[] names)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCalendarEventsByIndicators(string[] names)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCategories()
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetCountries()
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetByCountryAndPage(string country, int pageNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetBetweenCountries(string country1, string country2, int pageNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetHistorical(string symbol)
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> GetForecasts()
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetForecastsByDate(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetForecastsByCountries(string[] names)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetForecastsByCountriesAndDates(DateTime startDate, DateTime endDate, string[] names)
    {
        throw new NotImplementedException();
    }

    public async Task<object?> GetForecastsByIndicator(string[] names)
    {
        throw new NotImplementedException();
    }
}