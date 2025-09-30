namespace Orion.API.TradingEconomics.Interfaces;
public interface IComtradeServices
{
    Task<string> GetCategories();
    Task<string> GetCountries();
    Task<string> GetByCountryAndPage(string country, int page);
    Task<string> GetBetweenCountries(string country1, string country2, int page);
    Task<string> GetHistorical(string symbol);
}
