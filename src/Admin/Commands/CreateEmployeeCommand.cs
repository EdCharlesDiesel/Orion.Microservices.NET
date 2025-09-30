using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class CreateEmployeeCommand(IEmployeeFullEditDto values) : ICommand
    {
        public IEmployeeFullEditDto Values { get; private set; } = values;
    }
}
