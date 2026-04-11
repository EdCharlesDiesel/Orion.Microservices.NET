using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;


namespace Orion.Admin.Handlers
{
    public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
    {
        ICustomerRepository repo;
        IEventMediator mediator;
        public DeleteCustomerCommandHandler(ICustomerRepository repo, IEventMediator mediator)
        {
            this.repo = repo;
            this.mediator = mediator;
        }

        // FIXME HandleAsync(DeleteCustomerCommand 
        public async Task HandleAsync(DeleteCustomerCommand command)
        {
            // var deleted = await repo.Delete(command.CustomerId);
            // if (deleted != null)
            //     await mediator.TriggerEvents(deleted.DomainEvents);
            // await repo.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
