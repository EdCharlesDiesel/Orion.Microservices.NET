

namespace Orion.DataAccess.Postgres.Repositories;

public class ComtradeRepository: IComtradeServices
{
    public async Task<string> GetCategories()
        => await HttpRequesterClass.HttpRequester("/comtrade/categories");

    public async Task<string> GetCountries()
        => await HttpRequesterClass.HttpRequester("/comtrade/countries");

    public async Task<string> GetByCountryAndPage(string country, int page)
        => await HttpRequesterClass.HttpRequester($"/comtrade/country/{country}/{page}");

    public async Task<string> GetBetweenCountries(string country1, string country2, int page)
        => await HttpRequesterClass.HttpRequester($"/comtrade/country/{country1}/{country2}/{page}");

    public async Task<string> GetHistorical(string symbol)
        => await HttpRequesterClass.HttpRequester($"/comtrade/historical/{symbol}");
}

public interface IComtradeServices
{
}

public class HttpRequesterClass
{
    public static async Task<string> HttpRequester(string comtradeCountries)
    {
        throw new NotImplementedException();
    }
}
