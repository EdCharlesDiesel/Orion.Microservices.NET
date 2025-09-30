using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IProductReviewsRepository
{
    Task<IEnumerable<ProductReview>> GetAllAsync();
    Task<ProductReview?> GetByIdAsync(int id);
    Task AddAsync(ProductReview entity);
    void Update(ProductReview entity);
    void Delete(ProductReview entity);
}