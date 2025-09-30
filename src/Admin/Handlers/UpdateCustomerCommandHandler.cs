using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
    {
        ICustomerRepository repo;
        IEventMediator mediator;
        public UpdateCustomerCommandHandler(ICustomerRepository repo, IEventMediator mediator)
        {
            this.repo = repo;
            this.mediator = mediator;
        }

        // FIXME HandleAsync(UpdateCustomerCommand command)
        public async Task HandleAsync(UpdateCustomerCommand command)
        {
            // bool done = false;
            // ICustomer model = null;
            // while (!done)
            // {
            //     try
            //     {
            //         model = await repo.Get(command.Updates.Id);
            //         if (model == null) return;
            //         model.FullUpdate(command.Updates);
            //         await mediator.TriggerEvents(model.DomainEvents);
            //         await repo.UnitOfWork.SaveEntitiesAsync();
            //         done = true;
            //     }
            //     catch (DbUpdateConcurrencyException)
            //     {
            //
            //     }
            // }
            throw new NotImplementedException();
        }
    }
}
