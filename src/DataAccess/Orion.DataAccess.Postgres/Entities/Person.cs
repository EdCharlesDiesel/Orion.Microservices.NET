using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Person.Person")]
    [Description("Human beings involved with OrionProductionDatabase: employees, customer contacts, and vendor contacts.")]
    public class Person
    {
        public Person()
        {
            this.BusinessEntityContact = new List<BusinessEntityContact>();
            this.EmailAddresses = new List<EmailAddress>();
            this.PersonPhones = new List<PersonPhone>();
            this.Customers = new List<Customer>();
            this.PersonCreditCards = new List<PersonCreditCard>();
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for Person records.")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "PersonType", TypeName = "nchar")]
        [MaxLength(2)]
        [StringLength(2)]
        [Required(ErrorMessage = "Person Type is required")]
        [Display(Name = "Person Type")]
        [Description("Primary type of person: SC = Store Contact, IN = Individual (retail) customer, SP = Sales person, EM = Employee (non-sales), VC = Vendor contact, GC = General contact")]
        public string PersonType { get; set; } // nchar(2)
        [Column(name : "NameStyle", TypeName = "bit")]
        [Required(ErrorMessage = "Name Style is required")]
        [Display(Name = "Name Style")]
        [Description("0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.")]
       [NotMapped]
        public bool? NameStyle { get; set; } // bit
        [Column(name : "Title")]
        [MaxLength(8)]
        [StringLength(8)]
        [Display(Name = "Title")]
        [Description("A courtesy title. For example, Mr. or Ms.")]
        public string Title { get; set; } // nvarchar(8)
        [Column(name : "FirstName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [Description("First name of the person.")]
        public string FirstName { get; set; } // nvarchar(50)
        [Column(name : "MiddleName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Middle Name")]
        [Description("Middle name or middle initial of the person.")]
        public string MiddleName { get; set; } // nvarchar(50)
        [Column(name : "LastName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [Description("Last name of the person.")]
        public string LastName { get; set; } // nvarchar(50)
        [Column(name : "Suffix")]
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Name = "Suffix")]
        [Description("Surname suffix. For example, Sr. or Jr.")]
        public string Suffix { get; set; } // nvarchar(10)
        [Column(name : "EmailPromotion", TypeName = "int")]
        [Required(ErrorMessage = "Email Promotion is required")]
        [Display(Name = "Email Promotion")]
        [Description("0 = Contact does not wish to receive e-mail promotions, 1 = Contact does wish to receive e-mail promotions from OrionProductionDatabase, 2 = Contact does wish to receive e-mail promotions from OrionProductionDatabase and selected partners. ")]
        public int? EmailPromotion { get; set; } // int
        [Column(name : "AdditionalContactInfo", TypeName = "xml")]
        [Display(Name = "Additional Contact Info")]
        [Description("Additional contact information about the person stored in xml format. ")]
        public string AdditionalContactInfo { get; set; } // XML(.)
        [Column(name : "Demographics", TypeName = "xml")]
        [Display(Name = "Demographics")]
        [Description("Personal information such as hobbies, and income collected from online shoppers. Used for sales analysis.")]
        public string Demographics { get; set; } // XML(.)
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

        // Person.Person.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_Person_BusinessEntity_BusinessEntityID)
        // [ForeignKey("BusinessEntityID")]
        public BusinessEntity BusinessEntity { get; set; }
        // HumanResources.Employee.BusinessEntityID -> Person.Person.BusinessEntityID (FK_Employee_Person_BusinessEntityID)
        public Employee? Employee { get; set; }
        // Person.BusinessEntityContact.PersonID -> Person.Person.BusinessEntityID (FK_BusinessEntityContact_Person_PersonID)
        public IEnumerable<BusinessEntityContact> BusinessEntityContact { get; set; }
        // Person.EmailAddress.BusinessEntityID -> Person.Person.BusinessEntityID (FK_EmailAddress_Person_BusinessEntityID)
        public IEnumerable<EmailAddress> EmailAddresses { get; set; }
        // Person.Password.BusinessEntityID -> Person.Person.BusinessEntityID (FK_Password_Person_BusinessEntityID)
        public Password? Password { get; set; }
        // Person.PersonPhone.BusinessEntityID -> Person.Person.BusinessEntityID (FK_PersonPhone_Person_BusinessEntityID)
        public IEnumerable<PersonPhone> PersonPhones { get; set; }
        // Sales.Customer.PersonID -> Person.Person.BusinessEntityID (FK_Customer_Person_PersonID)
        public IEnumerable<Customer> Customers { get; set; }
        // Sales.PersonCreditCard.BusinessEntityID -> Person.Person.BusinessEntityID (FK_PersonCreditCard_Person_BusinessEntityID)
        public IEnumerable<PersonCreditCard> PersonCreditCards { get; set; }
    }
}
