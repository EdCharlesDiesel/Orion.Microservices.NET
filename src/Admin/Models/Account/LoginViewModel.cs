﻿using System.ComponentModel.DataAnnotations;

namespace Orion.Admin.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "user name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Display(Name = "remember me")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string EmailAddress { get; set; } 
        

        public string ReturnUrl { get; set; }
    }
}
