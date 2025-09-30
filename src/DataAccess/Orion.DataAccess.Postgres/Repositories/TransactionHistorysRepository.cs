using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class TransactionHistorysRepository(OrionDbContext context) : ITransactionHistorysRepository
{
    public async Task<IEnumerable<TransactionHistory>> GetAllAsync() =>
        await context.TransactionHistories.ToListAsync();

    public async Task<TransactionHistory?> GetByIdAsync(int id) =>
        await context.TransactionHistories.FindAsync(id);

    public async Task AddAsync(TransactionHistory entity) =>
        await context.TransactionHistories.AddAsync(entity);

    public void Update(TransactionHistory entity) =>
        context.TransactionHistories.Update(entity);

    public void Delete(TransactionHistory entity) =>
        context.TransactionHistories.Remove(entity);
}