using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrionDbContext _context;

        public EmployeeRepository(OrionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int businessEntityID)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.BusinessEntityID == businessEntityID);
        }

        public async Task AddEmployeeAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
        }

        public async Task UpdateEmployeeAsync(Employee entity)
        {
            _context.Employees.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteEmployeeAsync(Employee entity)
        {
            _context.Employees.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(Guid[] obligatoryCourseIds)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCalendarAsync(OrionCalendarEvent calendar)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(int[] obligatoryCourseIds)
        {
            return await _context.Courses
                .Where(c => obligatoryCourseIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task AddCalendarAsync(OrionCalendarEvent calendar)
        {
            await _context.OrionCalendarEvents.AddAsync(calendar);
        }

        public async Task<OrionCalendarEvent?> GetOrionCalendarEventAsync(int employeeId)
        {
            return await _context.OrionCalendarEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrionCalendarEvent>> GetOrionCalendarEventsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
