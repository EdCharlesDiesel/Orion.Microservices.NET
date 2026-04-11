using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface ITermRepository:IRepository<ITerm>
    {
        Task<ITerm> Get(int id);
        ITerm New();
    }
}
