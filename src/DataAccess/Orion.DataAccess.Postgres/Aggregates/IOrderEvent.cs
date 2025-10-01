using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum OrderEventType {Deleted}
    public interface IOrderEvent: IEntity<long>, IBaseEntity
    {
        OrderEventType Type { get; }
        int OrderId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
