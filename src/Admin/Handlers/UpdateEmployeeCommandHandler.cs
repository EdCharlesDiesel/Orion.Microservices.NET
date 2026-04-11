using Microsoft.EntityFrameworkCore;
using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand>
    {
        IEmployeeRepository repo;
        IEventMediator mediator;
        public UpdateEmployeeCommandHandler(IEmployeeRepository repo, IEventMediator mediator)
        {
            this.repo = repo;
            this.mediator = mediator;
        }
        public async Task HandleAsync(UpdateEmployeeCommand command)
        {
            // bool done = false;
            // IEmployee? model = null;
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
