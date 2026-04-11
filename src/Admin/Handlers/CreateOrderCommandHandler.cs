using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        IOrderRepository repo;
        public CreateOrderCommandHandler(IOrderRepository repo)
        {
            this.repo = repo;
        }
        public async Task  HandleAsync(CreateOrderCommand command)
        {
            // var model= repo.New();
            // model.FullUpdate(command.Values);
            // await repo.UnitOfWork.SaveEntitiesAsync();
            throw new NotImplementedException();
        }
    }
}
