using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.PhoneNumberType")]
    [Description("Type of phone number of a person.")]
    public class PhoneNumberType
    {
        public PhoneNumberType()
        {
            this.PersonPhones = new List<PersonPhone>();
        }

        [Key]
        [Column(name : "PhoneNumberTypeID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Phone Number Type ID is required")]
        [Display(Name = "Phone Number Type ID")]
        [Description("Primary key for telephone number type records.")]
        public int? PhoneNumberTypeID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Name of the telephone number type")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Person.PersonPhone.PhoneNumberTypeID -> Person.PhoneNumberType.PhoneNumberTypeID (FK_PersonPhone_PhoneNumberType_PhoneNumberTypeID)
        public IEnumerable<PersonPhone> PersonPhones { get; set; }
    }
}
