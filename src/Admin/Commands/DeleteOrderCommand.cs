using Orion.Admin.Tools;

namespace Orion.Admin.Commands
{
    public class DeleteOrderCommand(int id) : ICommand
    {
        public int OrderId { get; private set; } = id;
    }
}
