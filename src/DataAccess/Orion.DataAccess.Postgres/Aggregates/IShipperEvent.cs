using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum ShipperEventType {Deleted}
    public interface IShipperEvent: IEntity<long>, IBaseEntity
    {
        ShipperEventType Type { get; }
        int ShipperId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
