using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class UpdateEmployeeCommand(IEmployeeFullEditDto updates) : ICommand
    {
        public IEmployeeFullEditDto Updates { get; private set; } = updates;
    }
}
