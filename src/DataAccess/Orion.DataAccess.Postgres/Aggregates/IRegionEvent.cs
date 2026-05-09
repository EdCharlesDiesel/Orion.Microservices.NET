using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum RegionEventType {Deleted}
    public interface IRegionEvent: IEntity<long>, IBaseEntity
    {
        RegionEventType Type { get; }
        int RegionId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
