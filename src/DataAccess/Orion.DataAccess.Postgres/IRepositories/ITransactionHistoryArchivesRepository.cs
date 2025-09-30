using Orion.DataAccess.Postgres.Entities;
namespace Orion.DataAccess.Postgres.IRepositories;
public interface ITransactionHistoryArchivesRepository
{
    Task<IEnumerable<TransactionHistoryArchive>> GetAllAsync();
    Task<TransactionHistoryArchive?> GetByIdAsync(int id);
    Task AddAsync(TransactionHistoryArchive entity);
    void Update(TransactionHistoryArchive entity);
    void Delete(TransactionHistoryArchive entity);
}