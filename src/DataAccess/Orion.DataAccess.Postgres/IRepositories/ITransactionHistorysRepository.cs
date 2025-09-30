using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface ITransactionHistorysRepository
{
    Task<IEnumerable<TransactionHistory>> GetAllAsync();
    Task<TransactionHistory?> GetByIdAsync(int id);
    Task AddAsync(TransactionHistory entity);
    void Update(TransactionHistory entity);
    void Delete(TransactionHistory entity);
}