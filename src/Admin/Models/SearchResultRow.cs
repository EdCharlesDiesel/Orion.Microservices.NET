namespace Orion.Admin.Models
{
    public class SearchResultRow
    {
        public SearchResultRow(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
