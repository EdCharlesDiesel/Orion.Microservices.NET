
using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.Tools
{
    public interface IRepository
    {
    }
    public interface IRepository<T>: IRepository
    {
        IUnitOfWork UnitOfWork { get; }
        void GetByIdAsync(int i);
        
        List<Department> GetAllAsync();
      
        void AddAsync(Department department);
    }
}
