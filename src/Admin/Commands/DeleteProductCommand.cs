using Orion.Admin.Tools;

namespace Orion.Admin.Commands
{
    public class DeleteProductCommand: ICommand
    {
        public DeleteProductCommand(int id)
        {
            ProductId = id;
        }
        public int ProductId { get; private set; }
    }
}
