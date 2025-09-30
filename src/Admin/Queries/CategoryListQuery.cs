using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;

namespace Orion.Admin.Queries
{
    public class CategoryListQuery(OrionDbContext context) : ICategoryListQuery
    {
        public async Task<IEnumerable<SelectListItem>> AllCategories()
        {
            return (await context.Categories.Select(m => new
            {
                Text = m.CategoryName,
                Value = m.Id
            })
            .OrderBy(m => m.Text)
            .ToListAsync())
            .Select(m => new SelectListItem
            {
                Text = m.Text,
                Value = m.Value.ToString()
            });
        }
    }
}
