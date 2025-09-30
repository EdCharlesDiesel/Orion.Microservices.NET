using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.IRepositories
{
    public interface ICategoryRepository:IRepository<ICategory>
    {
        Task<ICategory> Get(int id);
        ICategory New();
    }
}
