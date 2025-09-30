namespace Orion.Admin.Models.Customers
{
    public abstract class CustomerInfosViewModel(
        int id,
        string companyName,
        string contactName,
        string contactTitle,
        string address,
        string city,
        string region,
        string postalCode,
        string country,
        string phone,
        string fax)
    {
        public int Id { get; set; } = id;


        public string CompanyName { get; set; } = companyName;


        public string ContactName { get; set; } = contactName;


        public string ContactTitle { get;set;  } = contactTitle;


        public string Address { get;set;  } = address;


        public string City { get;set;  } = city;


        public string Region { get;set;  } = region;


        public string PostalCode { get; set; } = postalCode;


        public string Country { get;set;  } = country;


        public string Phone { get; set; } = phone;


        public string Fax { get;set;  } = fax;

        //FIXME Fix tostring method
        // public override string ToString()
        // {
        //     return string.Format("{0}. {1} days in {2}, unit price: {3}",
        //         CustomerName, DurationInDays, CategoryName, UnitPrice);
        // }
    }
}
