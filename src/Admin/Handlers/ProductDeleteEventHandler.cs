using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class ProductDeleteEventHandler :
        IEventHandler<ProductDeleteEvent>
    {
        IProductEventRepository repo;
        public ProductDeleteEventHandler(IProductEventRepository repo)
        {
            this.repo = repo;
        }
        public Task HandleAsync(ProductDeleteEvent ev)
        {
            repo.New(ProductEventType.Deleted, ev.ProductId, ev.OldVersion);
            return Task.CompletedTask;
        }
    }

    public class ProductDeleteEvent : IEventNotification
    {
        public int ProductId { get; set; }
        public long OldVersion { get; set; }
    }
}
