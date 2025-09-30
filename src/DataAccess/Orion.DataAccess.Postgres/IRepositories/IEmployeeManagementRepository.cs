using System.Collections;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IEmployeeManagementRepository
{
    // Employee operations
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(Employee entity);
    Task UpdateEmployeeAsync(Employee entity);
    Task DeleteEmployeeAsync(Employee entity);

    // Course operations
    Task<IEnumerable<Course>> GetCoursesAsync(int[] obligatoryCourseIds);

    // Calendar operations
    Task AddCalendarAsync(OrionCalendarEvent calendar);
    Task<OrionCalendarEvent?> GetOrionCalendarEventAsync(int employeeId);

    // Persist changes
    Task SaveChangesAsync();
}
