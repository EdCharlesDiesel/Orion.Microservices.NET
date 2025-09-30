﻿namespace Orion.API.HumanResources.DataAccess.Entities
{
    public class Calendar : Employee
    { 
        [Required]
        public int YearsInService { get; set; }

        [NotMapped]
        public decimal SuggestedBonus { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public bool MinimumRaiseGiven { get; set; }

        public List<Course> AttendedCourses { get; set; } = new List<Course>();

        [Required]
        public int JobLevel { get; set; }

        public Calendar(
            string firstName,
            string lastName,
            int yearsInService,
            decimal salary,
            bool minimumRaiseGiven,
            int jobLevel)
            : base(firstName, lastName)
        {
            YearsInService = yearsInService;
            Salary = salary;
            MinimumRaiseGiven = minimumRaiseGiven;
            JobLevel = jobLevel;
        }

    }
}
