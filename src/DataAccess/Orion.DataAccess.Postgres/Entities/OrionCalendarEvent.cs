using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities;

/// <summary>
/// Orion Calendar of the database. 
/// </summary>
[Table("OrionCalendarEvent")]
public class OrionCalendarEvent
{
    [Key]
    [Column(name : "OrionCalendarEventID", TypeName = "int")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required(ErrorMessage = "Calendar Event ID is required")]
    [Display(Name = "Calendar Event ID")]
    [Description("Primary key for Calendar Event ID records.")]
    public int? OrionCalendarEventID{ get; set; } // int
    
    [Column(name : "Employee ID", TypeName = "int")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required(ErrorMessage = "Employee ID is required")]
    [Display(Name = "Employee ID")]

    public int? EmployeeID{ get; set; } // int
    
    
    public string Reference { get; set; }
    public DateTime? LastUpdate { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public Status Status { get; set; }
    public int JobLevel { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Salary { get; set; }
    public decimal SuggestedBonus { get; set; }
    public int YearsInService { get; set; }
}