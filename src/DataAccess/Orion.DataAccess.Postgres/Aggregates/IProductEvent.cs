using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Aggregates
{
    public enum ProductEventType {Deleted, PriceChanged}
    public interface IProductEvent: IEntity<long>, IBaseEntity
    {
        ProductEventType Type { get; }
        int ProductId { get;}
        decimal NewUnitPrice { get;}
        long? OldVersion { get;}
        long? NewVersion { get;}
    }
}
