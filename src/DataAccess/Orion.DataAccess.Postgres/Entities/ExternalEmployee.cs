using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("ExternalEmployees")]
    public class ExternalEmployee : Employee
    {
        [Required]
        [MaxLength(200)]
        public string Company { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string EmployeeNumber { get; set; } = null!;

        // Parameterless constructor for EF Core
        public ExternalEmployee() { }

        // Constructor for creating new external employees
        public ExternalEmployee(string firstName, string lastName, string company, string employeeNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            EmployeeNumber = employeeNumber;

            // Optional defaults
            Salary = 0;                 // external employees may not have a salary
            YearsInService = 0;
            SuggestedBonus = 0;
            MinimumRaiseGiven = false;
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}

