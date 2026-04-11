using System.Collections;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.Entities;
using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.IRepositories
{
    public interface IEmployeeRepository
    {
        // Employee operations
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee entity);
        Task UpdateEmployeeAsync(Employee entity);
        Task DeleteEmployeeAsync(Employee entity);

        // Course operations
        Task<IEnumerable<Course>> GetCoursesAsync(Guid[] obligatoryCourseIds);

        // Calendar operations
        Task CreateCalendarAsync(OrionCalendarEvent calendar);
        Task<OrionCalendarEvent?> GetOrionCalendarEventAsync(int employeeId);

        Task SaveChangesAsync();
        Task<IEnumerable<OrionCalendarEvent>> GetOrionCalendarEventsAsync();
    }
}