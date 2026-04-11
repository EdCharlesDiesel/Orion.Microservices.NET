using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class BasketRepository(OrionDbContext context, IBasketServices basketServicesImplementation) : IBasketServices
{
    private IBasketServices _basketServicesImplementation = basketServicesImplementation;
    // public async Task<Basket> Create(List<Basket> baskets)
    // {
    //     if (baskets == null)
    //         throw new ArgumentException("basket list cannot be null or empty.");
    //
    //     await context.Baskets.AddRangeAsync(baskets);
    //     await context.SaveChangesAsync();
    //
    //     return baskets.First();
    // }
    //
    // //TODO: Fix later
    // public async Task<IEnumerable<Basket>> GetAllAsync()
    // {
    //     var baskets = context.Baskets.ToList();
    //     if (baskets == null)
    //         throw new ArgumentException("basket list cannot be null or empty.");
    //     
    //     return baskets.ToList();
    //     
    // }
    //
    // async Task IRepository.GetByIdAsync(Guid id)
    // {
    //     await GetByIdAsync(id);
    // }
    //
    // public async Task AddAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // async Task IRepository.GetAllAsync()
    // {
    //     await GetAllAsync();
    // }
    //
    // async Task IRepository.GetByIdAsync(Guid id)
    // {
    //     await GetByIdAsync(id);
    // }
    //
    // public async Task AddAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // async Task IRepository.GetAllAsync()
    // {
    //     await GetAllAsync();
    // }
    //
    // async Task IRepository.GetByIdAsync(Guid id)
    // {
    //     await GetByIdAsync(id);
    // }
    //
    // public async Task AddAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // async Task IRepository.GetAllAsync()
    // {
    //     await GetAllAsync();
    // }
    //
    // async Task IRepository.GetByIdAsync(Guid id)
    // {
    //     await GetByIdAsync(id);
    // }
    //
    // public async Task AddAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    //
    // async Task IRepository.GetAllAsync()
    // {
    //     await GetAllAsync();
    // }
    //
    // async Task IRepository.GetAllAsync()
    // {
    //     await GetAllAsync();
    // }
    //
    // public async Task<Basket?> GetByIdAsync(Guid id)
    // {
    //     var baskets = context.Baskets.FindAsync(id);
    //     return await baskets;
    // }
    //
    // private async Task AddAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    //
    // public async Task AddAsync(Basket basket)
    // {
    //     if (basket == null)
    //         throw new ArgumentException("basket cannot be null or empty.");
    //     await context.Baskets.AddAsync(basket);
    //     await context.SaveChangesAsync();
    // }
    //
    // public async Task UpdateAsync(Basket basket)
    // {
    //     if (basket == null)
    //         throw new ArgumentException("basket cannot be null or empty.");
    //     context.Baskets.Update(basket);
    //     await context.SaveChangesAsync();
    //    
    // }
    //
    // public async Task UpdateAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    //
    //
    // public async Task UpdateAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task UpdateAsync(Basket entity)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task DeleteAsync(Guid id)
    // {
    //     var basket = await GetByIdAsync(id);
    //     if (basket == null)
    //         throw new ArgumentException("basket cannot be null or empty.");
    //     context.Baskets.Remove(basket);
    // }
    //
    // public async Task BulkCreate(List baskets)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task BulkCreate(List baskets)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task BulkCreate(List baskets)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task BulkCreate(List baskets)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task<List<Basket>?> BulkCreate(List<Basket> baskets)
    // {
    //     await context.AddRangeAsync(baskets);
    //     if (baskets == null)
    //         throw new ArgumentException("basket cannot be null or empty.");
    //     await context.AddRangeAsync(baskets);
    //     return null;
    // }
    //
    // public async Task<List< Basket>> BulkCreate(Basket baskets)
    // {
    //     if (baskets == null)
    //         throw new ArgumentException("basket cannot be null or empty.");
    //     await context.Baskets.AddRangeAsync(baskets);
    //     // await context.SaveChangesAsync();
    //     return null!;
    // }

    public async Task<IEnumerable<Basket>> GetAllAsync()
    {
        return await _basketServicesImplementation.GetAllAsync();
    }

    public async Task GetByIdAsync(Guid id)
    {
        await _basketServicesImplementation.GetByIdAsync(id);
    }

    public async Task AddAsync(Basket entity)
    {
        await _basketServicesImplementation.AddAsync(entity);
    }

    public async Task UpdateAsync(Basket entity)
    {
        await _basketServicesImplementation.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _basketServicesImplementation.DeleteAsync(id);
    }

    public async Task<List<Basket>?> BulkCreate(List<Basket> baskets)
    {
        return await _basketServicesImplementation.BulkCreate(baskets);
    }
}