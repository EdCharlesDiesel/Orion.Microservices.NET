using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class OrderEventRepository : IOrderEventRepository
    {
        //private OrionDbContext context;
        //public OrderEventRepository(OrionDbContext context)
        //{
        //    this.context = context;
        //}
        //public IUnitOfWork UnitOfWork => context;

        //public Task<IOrderEvent> Get(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IEnumerable<IOrderEvent>> GetFirstN(int n)
        //{
        //    return await context.OrderEvents
        //        .OrderBy(m => m.Id)
        //        .Take(n)
        //        .ToListAsync();
        //}

        //public IOrderEvent New(OrderEventType type, int id, long oldVersion, long? newVersion=null)
        //{
        //    var model = new OrderEvent
        //    {
        //        Type = type,
        //        OrderId = id,
        //        OldVersion = oldVersion,
        //        NewVersion = newVersion
        //    };
        //    context.OrderEvents.Add(model);
        //    return model;
        //}

        //public IOrderEvent New()
        //{
        //    throw new NotImplementedException();
        //}

        //public IOrderEvent New(OrderEventType deleted)
        //{
        //    throw new NotImplementedException();
        //}
        // public IUnitOfWork UnitOfWork => throw new NotImplementedException();
        //
        // public Task<IOrderEvent> Get(int id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public IOrderEvent New(OrderEventType deleted)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IEnumerable<IOrderEvent>> GetAllAsync()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IOrderEvent> GetByIdAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task AddAsync(IOrderEvent entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task UpdateAsync(IOrderEvent entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        public async Task<IEnumerable<IOrderEvent>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(IOrderEvent entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IOrderEvent entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IOrderEvent> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IOrderEvent New(OrderEventType deleted)
        {
            throw new NotImplementedException();
        }
    }
}
