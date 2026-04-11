using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.BusinessEntity")]
    [Description("Source of the ID that connects vendors, customers, and employees with address and contact information.")]
    public class BusinessEntity()
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for all customers, vendors, and employees.")]
        public int? BusinessEntityID  { get; set; } // int
        [Column(name : "rowguid")]
        [Required(ErrorMessage = "rowguid is required")]
        [Display(Name = "rowguid")]
        [Description("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")]
        public Guid? Rowguid { get; set; } // uniqueidentifier
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Person.BusinessEntityAddress.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_BusinessEntityAddress_BusinessEntity_BusinessEntityID)
        public IEnumerable<BusinessEntityAddress> BusinessEntityAddress { get; set; } = new List<BusinessEntityAddress>();

        // Person.BusinessEntityContact.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_BusinessEntityContact_BusinessEntity_BusinessEntityID)
        public IEnumerable<BusinessEntityContact> BusinessEntityContact { get; set; } = new List<BusinessEntityContact>();


    }
}
