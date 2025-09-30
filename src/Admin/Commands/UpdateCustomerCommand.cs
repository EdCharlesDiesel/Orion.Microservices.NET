using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class UpdateCustomerCommand(ICustomerFullEditDto updates) : ICommand
    {
        public ICustomerFullEditDto Updates { get; private set; } = updates;
    }
}
