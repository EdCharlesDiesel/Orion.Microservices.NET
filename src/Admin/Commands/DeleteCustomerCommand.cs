using Orion.Admin.Tools;

namespace Orion.Admin.Commands
{
    public class DeleteCustomerCommand(int id) : ICommand
    {
        public int CustomerId { get; private set; } = id;
    }
}
