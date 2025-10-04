using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Address", Schema = "Person")]
    [Description("Street address information for customers, employees, and vendors.")]
    public class Address
    {
        public Address()
        {
            this.BusinessEntityAddress = new List<BusinessEntityAddress>();
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
            this.SalesOrderHeaders1 = new List<SalesOrderHeader>();
        }

        [Key]
        [Column(name : "AddressID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Address ID is required")]
        [Display(Name = "Address ID")]
        [Description("Primary key for Address records.")]
        public int? AddressID { get; set; } // int
        [Column(name : "AddressLine1")]
        [MaxLength(60)]
        [StringLength(60)]
        [Required(ErrorMessage = "Address Line1 is required")]
        [Display(Name = "Address Line1")]
        [Description("First street address line.")]
        public string AddressLine1 { get; set; } // nvarchar(60)
        [Column(name : "AddressLine2")]
        [MaxLength(60)]
        [StringLength(60)]
        [Display(Name = "Address Line2")]
        [Description("Second street address line.")]
        public string AddressLine2 { get; set; } // nvarchar(60)
        [Column(name : "City")]
        [MaxLength(30)]
        [StringLength(30)]
        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        [Description("Name of the city.")]
        public string City { get; set; } // nvarchar(30)
        [Column(name : "StateProvinceID", TypeName = "int")]
        [Required(ErrorMessage = "State Province ID is required")]
        [Display(Name = "State Province ID")]
        [Description("Unique identification number for the state or province. Foreign key to StateProvince table.")]
        public int? StateProvinceID { get; set; } // int
        [Column(name : "PostalCode")]
        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        [Description("Postal code for the street address.")]
        public int PostalCode { get; set; } // nvarchar(15)
        [Column(name : "SpatialLocation", TypeName = "geography")]
        [Display(Name = "Spatial Location")]
        [Description("Latitude and longitude of this address.")]
        [NotMapped]
        public string SpatialLocation { get; set; } // geography
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

        // Person.Address.StateProvinceID -> Person.StateProvince.StateProvinceID (FK_Address_StateProvince_StateProvinceID)
        [ForeignKey("StateProvinceID")]
        public StateProvince? StateProvince { get; set; }
        // Person.BusinessEntityAddress.AddressID -> Person.Address.AddressID (FK_BusinessEntityAddress_Address_AddressID)
        public IEnumerable<BusinessEntityAddress> BusinessEntityAddress { get; set; }
        // Sales.SalesOrderHeader.BillToAddressID -> Person.Address.AddressID (FK_SalesOrderHeader_Address_BillToAddressID)
        [InverseProperty("Address")]
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
        // Sales.SalesOrderHeader.ShipToAddressID -> Person.Address.AddressID (FK_SalesOrderHeader_Address_ShipToAddressID)
        [InverseProperty("Address1")]
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders1 { get; set; }
    }
}
