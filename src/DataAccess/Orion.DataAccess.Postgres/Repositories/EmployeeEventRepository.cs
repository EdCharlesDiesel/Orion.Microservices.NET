﻿using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class EmployeeEventRepository : IEmployeeEventRepository
    {
        //private OrionDbContext context;
        //public EmployeeEventRepository(OrionDbContext context)
        //{
        //    this.context = context;
        //}
        //public IUnitOfWork UnitOfWork => context;

        //public object GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public object GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IEnumerable<IEmployeeEvent>> GetFirstN(int n)
        //{
        //    return await context.EmployeeEvents
        //        .OrderBy(m => m.Id)
        //        .Take(n)
        //        .ToListAsync();
        //}

        //public IEmployeeEvent New(EmployeeEventType type, int id, long oldVersion, long? newVersion = null, decimal unitPrice = 0)
        //{
        //    var model = new EmployeeEvent
        //    {
        //        Type = type,
        //        EmployeeId = id,
        //        OldVersion = oldVersion,
        //        NewVersion = newVersion,
        //        NewUnitPrice = unitPrice
        //    };
        //    context.EmployeeEvents.Add(model);
        //    return model;
        //}
        // public IUnitOfWork UnitOfWork { get; }
        // public Task<IEmployeeEvent> Get(int id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public IEmployeeEvent New()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IEnumerable<IEmployeeEvent>> GetAllAsync()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task<IEmployeeEvent> GetByIdAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task AddAsync(IEmployeeEvent entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task UpdateAsync(IEmployeeEvent entity)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public async Task DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
        public async Task<IEnumerable<IEmployeeEvent>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(IEmployeeEvent entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(IEmployeeEvent entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEmployeeEvent> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEmployeeEvent New()
        {
            throw new NotImplementedException();
        }
    }
}
