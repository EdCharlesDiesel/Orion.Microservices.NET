using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum SubscriptionEventType {Deleted}
    public interface ISubscriptionEvent: IEntity<long>, IBaseEntity
    {
        SubscriptionEventType Type { get; }
        int SubscriptionId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
