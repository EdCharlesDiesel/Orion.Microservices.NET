using System.ComponentModel.DataAnnotations;

namespace Orion.Admin.Models
{
    public class SearchViewModel
    {
        public SearchViewModel(string birthProvince, string businessProvince)
        {
            BirthProvince = birthProvince;
            BusinessProvince = businessProvince;
            Results = new List<SearchResultRow>();
            FirstName = String.Empty;
            LastName = String.Empty;
        }

        public List<SearchResultRow> Results { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Province")]
        public string BirthProvince { get; set; }

        [Display(Name = "Business Province")]
        public string BusinessProvince { get; set; }
    }
}
