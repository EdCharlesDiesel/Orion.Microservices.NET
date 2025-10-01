using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum RelationshipEventType {Deleted}
    public interface IRelationshipEvent: IEntity<long>, IBaseEntity
    {
        RelationshipEventType Type { get; }
        int RelationshipId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
