using Orion.Admin.Tools;

namespace Orion.Admin.Commands
{
    public class DeleteEmployeeCommand(int id) : ICommand
    {
        public int EmployeeId { get; private set; } = id;
    }
}
