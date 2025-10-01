using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum TerritoryEventType {Deleted}
    public interface ITerritoryEvent: IEntity<long>, IBaseEntity
    {
        TerritoryEventType Type { get; }

        int TerritoryId { get;}

        long? OldVersion { get;}
        
        long? NewVersion { get;}
    }
}
