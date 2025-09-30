using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        ICustomerRepository repo;
        public CreateCustomerCommandHandler(ICustomerRepository repo)
        {
            this.repo = repo;
        }
        // FIXME HandleAsync(CreateCustomerCommand 
        public async Task  HandleAsync(CreateCustomerCommand command)
        {
            var model= repo.New();
            // model.FullUpdate(command.Values);
            // await repo.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
