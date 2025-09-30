using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class UpdateOrderCommand(IOrderFullEditDto updates) : ICommand
    {
        public IOrderFullEditDto Updates { get; private set; } = updates;
    }
}
