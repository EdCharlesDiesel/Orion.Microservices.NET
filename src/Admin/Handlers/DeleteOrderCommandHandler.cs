using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
    {
        IOrderRepository repo;
        IEventMediator mediator;
        public DeleteOrderCommandHandler(IOrderRepository repo, IEventMediator mediator)
        {
            this.repo = repo;
            this.mediator = mediator;
        }
        public async Task HandleAsync(DeleteOrderCommand command)
        {
            // var deleted = await repo.Delete(command.OrderId);
            // if (deleted != null)
            //     await mediator.TriggerEvents(deleted.DomainEvents);
            // await repo.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
