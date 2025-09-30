using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.BusinessEntityContact")]
    [Description("Cross-reference table mapping stores, vendors, and employees to people")]
    public class BusinessEntityContact
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key. Foreign key to BusinessEntity.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "PersonID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Person ID is required")]
        [Display(Name = "Person ID")]
        [Description("Primary key. Foreign key to Person.BusinessEntityID.")]
        public int? PersonID { get; set; } // int
        [Key]
        [Column(name : "ContactTypeID", TypeName = "int", Order = 3)]
        [Required(ErrorMessage = "Contact Type ID is required")]
        [Display(Name = "Contact Type ID")]
        [Description("Primary key.  Foreign key to ContactType.ContactTypeID.")]
        public int? ContactTypeID { get; set; } // int
        [Column(name : "rowguid")]
        [Required(ErrorMessage = "rowguid is required")]
        [Display(Name = "rowguid")]
        [Description("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")]
        public Guid? rowguid { get; set; } // uniqueidentifier
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Person.BusinessEntityContact.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_BusinessEntityContact_BusinessEntity_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public BusinessEntity BusinessEntity { get; set; }
        // Person.BusinessEntityContact.PersonID -> Person.Person.BusinessEntityID (FK_BusinessEntityContact_Person_PersonID)
        [ForeignKey("BusinessEntityID")]
        public Person Person { get; set; }
        // Person.BusinessEntityContact.ContactTypeID -> Person.ContactType.ContactTypeID (FK_BusinessEntityContact_ContactType_ContactTypeID)
        [ForeignKey("ContactTypeID")]
        public ContactType ContactType { get; set; }
    }
}
