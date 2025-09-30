using System.Collections;
using System.Globalization;
using Orion.API.HumanResources.Business.EventArguments;
using Orion.API.HumanResources.Business.Exceptions;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;


namespace Orion.API.HumanResources.Business
{
    public abstract class EmployeeService : IEmployeeService
    {
        // Ids of obligatory courses: "Company Introduction" and "Respecting Your Colleagues" 
        private readonly Guid[] _obligatoryCourseIds = {
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e") };
        
        private readonly IEmployeeRepository _repository;
        private readonly EmployeeFactory _employeeFactory;
        public event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;
        
        public EmployeeService(IEmployeeRepository repository, EmployeeFactory employeeFactory)
        {
            _repository = repository;
            _employeeFactory = employeeFactory;
        }
        
        public async Task CreateOrionCalendarEventAsync(OrionCalendarEvent orionCalendarEvent)
        {
            await _repository.CreateCalendarAsync(orionCalendarEvent);
            await _repository.SaveChangesAsync();
        }
        public async Task AttendCourseAsync(Employee employee, Course attendedCourse)
        {
            var alreadyAttendedCourse = employee.AttendedCourses
                .Any(c => c.Id == attendedCourse.Id);
        
            if (alreadyAttendedCourse)
            {
                return;
            }
        
            // add course 
            employee.AttendedCourses.Append(attendedCourse);
        
            // save changes 
            await _repository.SaveChangesAsync();
        
            // execute business logic: when a course is attended, 
            // the suggested bonus must be recalculated
            employee.SuggestedBonus = employee.YearsInService
                * employee.AttendedCourses.Count() * 100;
        }

        public async Task<OrionCalendarEvent> CreateCalendarAsync(string firstName, string lastName, string company, string employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrionCalendarEvent?> FetchOrionCalendarEventAsync(int? employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrionCalendarEvent>> FetchOrionCalendarEventsAsync(int employeeId)
        {
            throw new NotImplementedException();
        }


        public async Task GiveMinimumRaiseAsync(Employee employee)
        {
            employee.Salary += 100;
            employee.MinimumRaiseGiven = true;
        
            // save this
            await _repository.SaveChangesAsync();
        }
        
        public async Task GiveRaiseAsync(Employee employee, int raise)
        {
            // raise must be at least 100
            if (raise < 100)
            {
                throw new EmployeeInvalidRaiseException(
                    message: "Invalid raise: raise must be higher than or equal to 100.", raise);
                //throw new Exception(
                //  "Invalid raise: raise must be higher than or equal to 100."); 
            }
        
            // if minimum raise was previously given, the raise must 
            // be higher than the minimum raise
            if (employee.MinimumRaiseGiven && raise == 100)
            {
                throw new EmployeeInvalidRaiseException(
                    "Invalid raise: minimum raise cannot be given twice.", raise);
            }
        
            if (raise == 100)
            {
                await GiveMinimumRaiseAsync(employee);
            }
            else
            {
                employee.Salary += raise;
                employee.MinimumRaiseGiven = false;
                await _repository.SaveChangesAsync();
            }
        }
        
        public async Task<OrionCalendarEvent?> FetchOrionCalendarEventAsync(int employeeId)
        {
            var employee = await _repository.GetOrionCalendarEventAsync(employeeId);
        
            if (employee != null)
            {
                // calculate fields
                // employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }
            return employee;
        }
        
        public async Task<IEnumerable<OrionCalendarEvent>> FetchOrionCalendarEventsAsync(Guid employeeId)
        {
            var calendars = await _repository.GetOrionCalendarEventsAsync();
        
            return calendars;
        }
        public async Task<OrionCalendarEvent> CreateCalendarForEmployeeAsync(string firstName, string lastName)
        {
            // use the factory to create the object 
            var employee = _employeeFactory.CreateEmployee(
                firstName, lastName);
        
            // apply business logic 
        
            // add obligatory courses attended by all new employees
            // during vetting process
        
            // get those courses  
            var obligatoryCourses = await _repository.GetCoursesAsync(
                _obligatoryCourseIds);
        
            // add them for this employee
            foreach (var obligatoryCourse in obligatoryCourses)
            {
                employee.AttendedCourses.Append(obligatoryCourse);
            }
        
            // calculate the suggested bonus
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            return new OrionCalendarEvent();
        }

        public void NotifyOfAbsence(Employee employee)
        {
            // Employee is absent.  Other parts of the application may 
            // respond to this. Trigger the EmployeeIsAbsent event 
            // (via a virtual method so it can be overridden in subclasses)
            OnEmployeeIsAbsent(new EmployeeIsAbsentEventArgs(employee.BusinessEntityID));
        }
        
        protected virtual void OnEmployeeIsAbsent(EmployeeIsAbsentEventArgs eventArgs)
        {
            EmployeeIsAbsent?.Invoke(this, eventArgs);
        }
        
        private int CalculateSuggestedBonus(Employee employee)
        {
            if (employee.YearsInService == 0)
            {
                return employee.AttendedCourses.Count() * 100;
            }
        
            return employee.YearsInService
                   * employee.AttendedCourses.Count() * 100;
        }
        
                
        // public ExternalEmployee CreateExternalEmployee(
        //     string firstName, string lastName, string company,string employeeNumber)
        // {
        //     // create a new external employee with default values 
        //     var employee = _employeeFactory.CreateEmployee(
        //         firstName, lastName, company,employeeNumber,true);
        //
        //     // no obligatory courses for external employees, return it
        //     return employee;
        // }

       
    }

}
