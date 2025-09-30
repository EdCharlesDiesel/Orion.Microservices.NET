using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IShoppingCartItemsRepository
{
    Task<IEnumerable<ShoppingCartItem>> GetAllAsync();
    Task<ShoppingCartItem?> GetByIdAsync(int id);
    Task AddAsync(ShoppingCartItem entity);
    void Update(ShoppingCartItem entity);
    void Delete(ShoppingCartItem entity);
}