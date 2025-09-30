﻿using Orion.API.HumanResources.DataAccess.Entities;

namespace Orion.API.HumanResources.DataAccess.Services
{
    public interface IEmployeeManagementRepository
    {
        Task<IEnumerable<Calendar>> GetCalendarsAsync();

        Calendar? GetCalendar(Guid employeeId);

        Task<Calendar?> GetCalendarAsync(Guid employeeId);

        Task<Course?> GetCourseAsync(Guid courseId);

        Course? GetCourse(Guid courseId);

        List<Course> GetCourses(params Guid[] courseIds);

        Task<List<Course>> GetCoursesAsync(params Guid[] courseIds);

        void AddCalendar(Calendar Calendar);

        Task SaveChangesAsync();
    }
}