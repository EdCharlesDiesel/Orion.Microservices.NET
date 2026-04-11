using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class ProductUnitPriceChangedEventHandler :
        IEventHandler<ProductUnitPriceChangedEvent>
    {
        IProductEventRepository repo;
        public ProductUnitPriceChangedEventHandler(IProductEventRepository repo)
        {
            this.repo = repo;
        }
        public Task HandleAsync(ProductUnitPriceChangedEvent ev)
        {
            repo.New(ProductEventType.PriceChanged, ev.ProductId, ev.OldVersion, ev.NewVersion, ev.NewUnitPrice);
            return Task.CompletedTask;
        }
    }

    public class ProductUnitPriceChangedEvent : IEventNotification
    {
        public long OldVersion { get; set; }
        public int ProductId { get; set; }
        public long? NewVersion { get; set; }
        public decimal NewUnitPrice { get; set; }
    }
}
