using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // private OrionDbContext context;
        // public OrderRepository(OrionDbContext context)
        // {
        //     this.context = context;
        // }
        // public IUnitOfWork UnitOfWork => context;
        //
        // public async Task<IOrder> Get(int id)
        // {
        //     return await context.Orders.Where(m => m.Id == id)
        //         .FirstOrDefaultAsync();
        // }
        //
        // public async Task<IOrder> Delete(int id)
        // {
        //     var model = await Get(id);
        //     if (model == null) return null;
        //     context.Orders.Remove(model as Order);
        //     model.AddDomainEvent(
        //         new OrderDeleteEvent(
        //             model.Id, (model as Order).EntityVersion));
        //     return model;
        // }
        //
        // public IOrder New()
        // {
        //     var model = new Order { EntityVersion = 1 };
        //     context.Orders.Add(model);
        //     return model;
        // }
        //
        // Task IOrderRepository.Delete(int orderId)
        // {
        //     throw new NotImplementedException();
        // }
        // public IUnitOfWork UnitOfWork => throw new NotImplementedException();
        //
        // public Task Delete(int orderId)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public Task<IOrder> Get(int id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public IOrder New()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IEnumerable<IOrder>> GetAllAsync()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IOrder> GetByIdAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task AddAsync(IOrder entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task UpdateAsync(IOrder entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        public async Task<IEnumerable<IOrder>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(IOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IOrder> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IOrder New()
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int orderId)
        {
            throw new NotImplementedException();
        }

        public object UnitOfWork { get; set; }
    }
}
