using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        //private OrionDbContext context;
        //public CustomerRepository(OrionDbContext context)
        //{
        //    this.context = context;
        //}
        //public IUnitOfWork UnitOfWork => context;

        //public async Task<ICustomer> Get(int id)
        //{
        //    return await context.Customers.Where(m => m.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<ICustomer> Delete(int id)
        //{
        //    var model = await Get(id);
        //    if (model == null) return null;
        //    context.Customers.Remove(model as Customer);
        //    model.AddDomainEvent(
        //        new CustomerDeleteEvent(
        //            model.Id, (model as Customer).EntityVersion));
        //    return model;
        //}

        //public ICustomer New()
        //{
        //    var model = new Customer() { EntityVersion = 1 };
        //    context.Customers.Add(model);
        //    return model;
        //}

        //public object GetAll()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public object GetById(int id)
        //{
        //    throw new System.NotImplementedException();
        //}

        public async Task<IEnumerable<ICustomer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(ICustomer entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ICustomer entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICustomer> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ICustomer New()
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
