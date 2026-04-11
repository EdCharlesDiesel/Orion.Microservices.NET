using System.ComponentModel.DataAnnotations;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.Domain.DTOs;

namespace Orion.Admin.Models.Employees
{
    public class EmployeeFullEditViewModel: IEmployeeFullEditDto
    {
        public EmployeeFullEditViewModel(string lastName, string firstName, string title, string titleOfCourtesy, string address, string city, string region, string postalCode, string country, string homePhone, string extension, byte[] photo, string notes, string photoPath)
        {
            LastName = lastName;
            FirstName = firstName;
            Title = title;
            TitleOfCourtesy = titleOfCourtesy;
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            HomePhone = homePhone;
            Extension = extension;
            Photo = photo;
            Notes = notes;
            PhotoPath = photoPath;
        }
        public EmployeeFullEditViewModel(IEmployee o, string lastName, string firstName, string title, string titleOfCourtesy, string address, string city, string region, string postalCode, string country, string homePhone, string extension, byte[] photo, string notes, string photoPath)
        {
            Extension = extension;
            Notes = notes;
            Id = o.Id;
            LastName = o.LastName;
            FirstName = o.FirstName;
            Title = o.Title;
            TitleOfCourtesy = o.TitleOfCourtesy;
            BirthDate = o.BirthDate;
            HireDate = o.HireDate;
            Address = o.Address;
            City = o.City;
            Region = o.Region;
            PostalCode = o.PostalCode;
            Country = o.Country;
            HomePhone = o.HomePhone;
            Photo = o.Photo;
            ReportsTo = o.ReportsTo;
            PhotoPath = o.PhotoPath;
        }
        public int Id { get; set; }
        [MaxLength(20)]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(25)]
        public string TitleOfCourtesy { get; set; }


        public DateTime? BirthDate { get; set; }


        public DateTime? HireDate { get; set; }


        [MaxLength(60)]
        public string Address { get; set; }


        [MaxLength(15)]
        public string City { get; set; }


        [MaxLength(15)]
        public string Region { get; set; }


        [MaxLength(10)]
        public string PostalCode { get; set; }


        [MaxLength(15)]
        public string Country { get; set; }


        [MaxLength(24)]
        public string HomePhone { get; set; }


        [MaxLength(4)]
        public string Extension { get; set; }


        public byte[] Photo { get; set; }


        public string Notes { get; set; }


        public int? ReportsTo { get; set; }


        [MaxLength(255)]
        public string PhotoPath { get; set; } 


        [Display(Name = "EmployeeTerritory")]
        public int EmployeeTerritoryId { get; set; }
        
        [Display(Name = "Order")]
        public int OrderId { get; set; }
    }
}
