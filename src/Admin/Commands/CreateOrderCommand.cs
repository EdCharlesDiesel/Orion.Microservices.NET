using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class CreateOrderCommand(IOrderFullEditDto values) : ICommand
    {
        public IOrderFullEditDto Values { get; private set; } = values;
    }
}
