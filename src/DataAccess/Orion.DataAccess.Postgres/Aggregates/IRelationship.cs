using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.DTOs;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public interface IRelationship: IEntity<int>, IBaseEntity
    {
        void FullUpdate(IRelationshipFullEditDto o);

        int FromPersonId { get;}     

        int ToPersonId { get;}     

        string RelationshipType { get;}
    }   
}

 