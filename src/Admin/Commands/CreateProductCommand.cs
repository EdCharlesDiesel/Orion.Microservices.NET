using Orion.Admin.Tools;
using Orion.Domain.DTOs;

namespace Orion.Admin.Commands
{
    public class CreateProductCommand(IProductFullEditDto values) : ICommand
    {
        public IProductFullEditDto Values { get; private set; } = values;
    }
}
