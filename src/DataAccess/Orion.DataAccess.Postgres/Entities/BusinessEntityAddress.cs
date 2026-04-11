using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.BusinessEntityAddress")]
    [Description("Cross-reference table mapping customers, vendors, and employees to their addresses.")]
    public class BusinessEntityAddress
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key. Foreign key to BusinessEntity.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "AddressID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Address ID is required")]
        [Display(Name = "Address ID")]
        [Description("Primary key. Foreign key to Address.AddressID.")]
        public int? AddressID { get; set; } // int
        [Key]
        [Column(name : "AddressTypeID", TypeName = "int", Order = 3)]
        [Required(ErrorMessage = "Address Type ID is required")]
        [Display(Name = "Address Type ID")]
        [Description("Primary key. Foreign key to AddressType.AddressTypeID.")]
        public int? AddressTypeID { get; set; } // int
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

        // Person.BusinessEntityAddress.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_BusinessEntityAddress_BusinessEntity_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public BusinessEntity BusinessEntity { get; set; }
        // Person.BusinessEntityAddress.AddressID -> Person.Address.AddressID (FK_BusinessEntityAddress_Address_AddressID)
        [ForeignKey("AddressID")]
        public Address Address { get; set; }
        // Person.BusinessEntityAddress.AddressTypeID -> Person.AddressType.AddressTypeID (FK_BusinessEntityAddress_AddressType_AddressTypeID)
        [ForeignKey("AddressTypeID")]
        public AddressType AddressType { get; set; }
    }
}
