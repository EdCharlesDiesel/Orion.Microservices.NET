using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.PersonPhone")]
    [Description("Telephone number and type of a person.")]
    public class PersonPhone
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Business entity identification number. Foreign key to Person.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "PhoneNumber", Order = 2)]
        [MaxLength(25)]
        [StringLength(25)]
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        [Description("Telephone number identification number.")]
        public string PhoneNumber { get; set; } // nvarchar(25)
        [Key]
        [Column(name : "PhoneNumberTypeID", TypeName = "int", Order = 3)]
        [Required(ErrorMessage = "Phone Number Type ID is required")]
        [Display(Name = "Phone Number Type ID")]
        [Description("Kind of phone number. Foreign key to PhoneNumberType.PhoneNumberTypeID.")]
        public int? PhoneNumberTypeID { get; set; } // int
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Person.PersonPhone.BusinessEntityID -> Person.Person.BusinessEntityID (FK_PersonPhone_Person_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Person Person { get; set; }
        // Person.PersonPhone.PhoneNumberTypeID -> Person.PhoneNumberType.PhoneNumberTypeID (FK_PersonPhone_PhoneNumberType_PhoneNumberTypeID)
        [ForeignKey("PhoneNumberTypeID")]
        public PhoneNumberType PhoneNumberType { get; set; }
    }
}
