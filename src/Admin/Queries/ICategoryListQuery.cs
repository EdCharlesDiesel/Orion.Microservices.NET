using Microsoft.AspNetCore.Mvc.Rendering;
using Orion.Admin.Tools;

namespace Orion.Admin.Queries
{
    public interface ICategoryListQuery: IQuery
    {
        Task<IEnumerable<SelectListItem>> AllCategories();
    }
}
