using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.Password")]
    [Description("One way hashed authentication information")]
    public class Password
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "PasswordHash", TypeName = "varchar")]
        [MaxLength(128)]
        [StringLength(128)]
        [Required(ErrorMessage = "Password Hash is required")]
        [Display(Name = "Password Hash")]
        [Description("Password for the e-mail account.")]
        public string PasswordHash { get; set; } // varchar(128)
        [Column(name : "PasswordSalt", TypeName = "varchar")]
        [MaxLength(10)]
        [StringLength(10)]
        [Required(ErrorMessage = "Password Salt is required")]
        [Display(Name = "Password Salt")]
        [Description("Random value concatenated with the password string before the password is hashed.")]
        public string PasswordSalt { get; set; } // varchar(10)
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

        // Person.Password.BusinessEntityID -> Person.Person.BusinessEntityID (FK_Password_Person_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Person Person { get; set; }
    }
}
