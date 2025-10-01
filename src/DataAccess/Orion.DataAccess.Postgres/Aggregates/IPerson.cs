using Orion.DataAccess.Postgres.Services;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IPerson: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IPersonFullEditDto o);

        string FirstName { get; set; }

        string LastName { get; set; }
        
    }

    public interface IPersonFullEditDto
    {
    }
}

 