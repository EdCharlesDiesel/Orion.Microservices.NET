using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("BuildVersion")]
    [Description("Current version number of the OrionProductionDatabase database. ")]
    public class BuildVersion(string databaseVersion)
    {
        [Key]
        [Column(name : "SystemInformationID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "System Information ID is required")]
        [Display(Name = "System Information ID")]
        [Description("Primary key for BuildVersion records.")]
        public int? SystemInformationID { get; set; } // tinyint
        [Column(name : "Database Version")]
        [MaxLength(25)]
        [StringLength(25)]
        [Display(Name = "Database Version")]
        [Description("Version number of the database in 9.yy.mm.dd.00 format.")]
        public string? DatabaseVersion { get; set; }
        [Column(name : "VersionDate")]
        [Required(ErrorMessage = "Version Date is required")]
        [Display(Name = "Version Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? VersionDate { get; set; } // datetime
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime
    }
}
