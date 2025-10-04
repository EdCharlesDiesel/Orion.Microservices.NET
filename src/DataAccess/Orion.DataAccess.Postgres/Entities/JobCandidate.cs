using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("JobCandidate",Schema = "HumanResources")]
    [Description("Résumés submitted to Human Resources by job applicants.")]
    public class JobCandidate
    {
        [Key]
        [Column(name : "JobCandidateID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Job Candidate ID is required")]
        [Display(Name = "Job Candidate ID")]
        [Description("Primary key for JobCandidate records.")]
        public int? JobCandidateID { get; set; } // int
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Display(Name = "Business Entity ID")]
        [Description("Employee identification number if applicant was hired. Foreign key to Employee.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "Resume")]
        [Display(Name = "Resume")]
        [Description("Résumé in XML format.")]
        public string Resume { get; set; } 
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // HumanResources.JobCandidate.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_JobCandidate_Employee_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
    }
}
