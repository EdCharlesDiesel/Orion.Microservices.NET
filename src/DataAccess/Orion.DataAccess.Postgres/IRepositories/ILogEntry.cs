using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ILogEntryRepository:IRepository<ILogEntry>
    {
        Task<ILogEntry> Get(int id);
        ILogEntry New();
    }
}
