using Orion.Admin.Commands;
using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.Domain.IRepositories;

namespace Orion.Admin.Handlers
{
    public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand>
    {
        IEmployeeRepository repo;
        public CreateEmployeeCommandHandler(IEmployeeRepository repo)
        {
            this.repo = repo;
        }
        // FIXME HandleAsync(CreateEmployeeCommand 
        public async Task  HandleAsync(CreateEmployeeCommand command)
        {
            // var model= repo.New();
            // model.FullUpdate(command.Values);
            // await repo.UnitOfWork.SaveEntitiesAsync();
            throw new NotImplementedException();
        }
    }
}
