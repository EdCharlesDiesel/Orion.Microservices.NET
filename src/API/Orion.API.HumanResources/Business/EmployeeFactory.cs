

using Orion.DataAccess.Postgres.Entities;

namespace Orion.API.HumanResources.Business
{
    /// <summary>
    /// Factory for creation employees
    /// </summary>
    public class EmployeeFactory
    {
        /// <summary>
        /// Create an employee
        /// </summary>
        public virtual Employee CreateEmployee(string firstName,
            string lastName, 
            string? company = null,
            string? employeeID = null,
            bool isExternal = false)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"'{nameof(firstName)}' cannot be null or empty.", 
                    nameof(firstName));
            }
    
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"'{nameof(lastName)}' cannot be null or empty.", 
                    nameof(lastName));
            }
    
            if (company == null && isExternal)
            {
                throw new ArgumentException($"'{nameof(company)}' cannot be null or empty when the employee is external.",
                    nameof(company));
            }
            
            if (string.IsNullOrEmpty(employeeID))
            {
                throw new ArgumentException($"'{nameof(employeeID)}' cannot be null or empty when the employee is external.",
                    nameof(employeeID));
            }
    
            if (isExternal)
            {
                // we know company won't be null here due to the check above, so 
                // we can use the null-forgiving operator to notify the compiler of this
                return new ExternalEmployee();
            }
    
            // create a new employee with default values 
            // return new OrionCalendarEvent();
            //TODO: Need to fi this it's in correct.
            return  new Employee();
        }
    }
    



}
