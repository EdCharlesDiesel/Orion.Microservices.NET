using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IPerson: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IPersonFullEditDto o);

        string FirstName { get; set; }

        string LastName { get; set; }
        
    }   
}

 