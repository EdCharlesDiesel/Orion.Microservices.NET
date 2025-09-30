using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class UpdateProductCommand(IProductFullEditDto updates) : ICommand
    {
        public IProductFullEditDto Updates { get; private set; } = updates;
    }
}
