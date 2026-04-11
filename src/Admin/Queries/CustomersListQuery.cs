using Orion.Admin.Models.Customers;
using Orion.DataAccess.Postgres.Data;

namespace Orion.Admin.Queries
{
    public class CustomersListQuery:ICustomersListQuery
    {
        OrionDbContext context;
        public CustomersListQuery(OrionDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<CustomerInfosViewModel>> GetAllCustomers()
        {

            // return await context.Customers.Select(m => new CustomerInfosViewModel
            // {
            //     // StartValidityDate = m.StartValidityDate,
            //     // EndValidityDate = m.EndValidityDate,
            //     // CustomerName = m.CustomerName,
            //     // DurationInDays = m.DurationInDays,
            //     Id = m.Id,
            //     // Image= m.Image,
            //     // UnitPrice = m.UnitPrice,
            //     // CategoryId = m.CategoryId
            // })
            //   //  .OrderByDescending(m=> m.EndValidityDate)
            //     .ToListAsync();
            throw new NotImplementedException();
        }
    }
}
