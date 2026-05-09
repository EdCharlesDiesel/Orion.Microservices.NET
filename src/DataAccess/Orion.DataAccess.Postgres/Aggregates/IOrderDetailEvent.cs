using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum OrderDetailEventType {Deleted}
    public interface IOrderDetailEvent: IEntity<long>, IBaseEntity
    {
        OrderDetailEventType Type { get; }
        int OrderDetailId { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
