using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IDocumentsRepository
{
    Task<IEnumerable<Document>> GetAllAsync();
    Task<Document?> GetByIdAsync(int id);
    Task AddAsync(Document entity);
    void Update(Document entity);
    void Delete(Document entity);
}