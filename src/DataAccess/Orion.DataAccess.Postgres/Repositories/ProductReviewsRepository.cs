using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class ProductReviewsRepository(OrionDbContext context) : IProductReviewsRepository
{
    
    public async Task<IEnumerable<ProductReview>> GetAllAsync() =>
        await context.ProductReviews.ToListAsync();

    public async Task<ProductReview?> GetByIdAsync(int id) =>
        await context.ProductReviews.FindAsync(id);

    public async Task AddAsync(ProductReview entity) =>
        await context.ProductReviews.AddAsync(entity);

    public void Update(ProductReview entity) =>
        context.ProductReviews.Update(entity);

    public void Delete(ProductReview entity) =>
        context.ProductReviews.Remove(entity);
}