namespace Orion.API.TradingEconomics.Interfaces
{


    public interface IRepository<T>
    {
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
