using System.Globalization;
using Orion.API.HumanResources.Business.EventArguments;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.API.HumanResources.Business
{
    public interface IEmployeeService
    {
        event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;
        Task CreateOrionCalendarEventAsync(OrionCalendarEvent calendar);
        Task AttendCourseAsync(Employee employee, Course attendedCourse);
        Task<OrionCalendarEvent> CreateCalendarAsync(string firstName, string lastName, string company,string employeeId);
        Task<OrionCalendarEvent?> FetchOrionCalendarEventAsync(int? employeeId);
        Task<IEnumerable<OrionCalendarEvent>> FetchOrionCalendarEventsAsync(int employeeId);
        Task GiveMinimumRaiseAsync(Employee employee);
        Task GiveRaiseAsync(Employee employee, int raise);
        void NotifyOfAbsence(Employee employee);
    }


}