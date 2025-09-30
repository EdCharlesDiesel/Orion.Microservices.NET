using System;

namespace Orion.Domain.DTO;

public class OrionCalendarEventDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmployeeNumber { get; set; }
    public string Company { get; set; }
    public object Salary { get; set; }
    public object SuggestedBonus { get; set; }
    public object YearsInService { get; set; }
    public Guid Id { get; set; }
}