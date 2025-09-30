using System.Threading.Tasks;
using Orion.DataAccess.Postgres.Aggregates;

namespace Orion.Domain.IRepositories
{
    public interface IRegionRepository:IRepository<IRegion>
    {
        Task<IRegion> Get(int id);
        IRegion New();
    }
}
