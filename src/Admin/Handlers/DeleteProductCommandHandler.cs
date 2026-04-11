using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        IProductRepository repo;
        IEventMediator mediator;
        public DeleteProductCommandHandler(IProductRepository repo, IEventMediator mediator)
        {
            this.repo = repo;
            this.mediator = mediator;
        }
        public async Task HandleAsync(DeleteProductCommand command)
        {
            // var deleted = await repo.Delete(command.ProductId);
            // if (deleted != null)
            //     await mediator.TriggerEvents(deleted.DomainEvents);
            // await repo.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
