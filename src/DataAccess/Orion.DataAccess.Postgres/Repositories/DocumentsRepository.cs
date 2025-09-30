using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class DocumentsRepository(OrionDbContext context) : IDocumentsRepository
{
    
    public async Task<IEnumerable<Document>> GetAllAsync() =>
        await context.Documents.ToListAsync();

    public async Task<Document?> GetByIdAsync(int id) =>
        await context.Documents.FindAsync(id);

    public async Task AddAsync(Document entity) =>
        await context.Documents.AddAsync(entity);

    public void Update(Document entity) =>
        context.Documents.Update(entity);

    public void Delete(Document entity) =>
        context.Documents.Remove(entity);
}