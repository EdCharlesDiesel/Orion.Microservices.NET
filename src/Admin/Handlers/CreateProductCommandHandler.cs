using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        IProductRepository repo;
        public CreateProductCommandHandler(IProductRepository repo)
        {
            this.repo = repo;
        }
        public async Task  HandleAsync(CreateProductCommand command)
        {
            // var model= repo.New();
            // model.FullUpdate(command.Values);
            // await repo.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
