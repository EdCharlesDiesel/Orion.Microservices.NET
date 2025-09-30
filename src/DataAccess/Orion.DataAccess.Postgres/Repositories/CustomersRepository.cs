using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;
public class CustomersRepository(OrionDbContext context) : ICustomersRepository
{
    
    public async Task<IEnumerable<Customer>> GetAllAsync() =>
        await context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id) =>
        await context.Customers.FindAsync(id);

    public async Task AddAsync(Customer entity) =>
        await context.Customers.AddAsync(entity);

    public void Update(Customer entity) =>
        context.Customers.Update(entity);

    public void Delete(Customer entity) =>
        context.Customers.Remove(entity);
}