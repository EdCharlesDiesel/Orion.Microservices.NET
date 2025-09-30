using System.ComponentModel.DataAnnotations;
using Orion.Admin.Controllers;


namespace Orion.Admin.Models.Account
{
    public class MasterUserEditViewModel
    {
        [Required(ErrorMessage = "Must type into user name")]
        [Display(Name = "User Name")]
        [MinLength(3, ErrorMessage = "Minimum lengnt is 3")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Must type into password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public MasterUserEditViewModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public MasterUserEditViewModel(MasterUser appUser, string userName, string password)
        {
            UserName = appUser.UserName;
            Password = appUser.PasswordHash;
        }

        public MasterUserEditViewModel(MasterUser? userName)
        {
            throw new NotImplementedException();
        }
    }
}
