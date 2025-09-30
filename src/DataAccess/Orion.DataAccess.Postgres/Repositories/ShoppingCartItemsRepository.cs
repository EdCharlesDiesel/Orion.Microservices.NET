using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;


public class ShoppingCartItemsRepository(OrionDbContext context) : IShoppingCartItemsRepository
{
    public async Task<IEnumerable<ShoppingCartItem>> GetAllAsync() =>
        await context.ShoppingCartItems.ToListAsync();

    public async Task<ShoppingCartItem?> GetByIdAsync(int id) =>
        await context.ShoppingCartItems.FindAsync(id);

    public async Task AddAsync(ShoppingCartItem entity) =>
        await context.ShoppingCartItems.AddAsync(entity);

    public void Update(ShoppingCartItem entity) =>
        context.ShoppingCartItems.Update(entity);

    public void Delete(ShoppingCartItem entity) =>
        context.ShoppingCartItems.Remove(entity);
}