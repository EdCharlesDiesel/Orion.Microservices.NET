using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class CreateCustomerCommand(ICustomerFullEditDto values) : ICommand
    {
        public ICustomerFullEditDto Values { get; private set; } = values;
    }
}
