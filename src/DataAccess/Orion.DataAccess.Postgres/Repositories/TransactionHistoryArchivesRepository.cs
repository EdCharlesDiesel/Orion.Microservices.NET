using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class TransactionHistoryArchivesRepository(OrionDbContext context): ITransactionHistoryArchivesRepository
{
    public async Task<IEnumerable<TransactionHistoryArchive>> GetAllAsync() =>
        await context.TransactionHistoryArchives.ToListAsync();

    public async Task<TransactionHistoryArchive?> GetByIdAsync(int id) =>
        await context.TransactionHistoryArchives.FindAsync(id);

    public async Task AddAsync(TransactionHistoryArchive entity) =>
        await context.TransactionHistoryArchives.AddAsync(entity);

    public void Update(TransactionHistoryArchive entity) =>
        context.TransactionHistoryArchives.Update(entity);

    public void Delete(TransactionHistoryArchive entity) =>
        context.TransactionHistoryArchives.Remove(entity);
}