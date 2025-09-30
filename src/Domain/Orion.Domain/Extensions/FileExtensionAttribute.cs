﻿using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;


namespace Orion.Domain.Extensions
{
    // FIXME IForm Download package
    public class FileExtensionAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                string[] extensions = { "jpg", "png", "jpeg" };

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result) new ValidationResult(ErrorMessage = "Allowed extension are jpg, png, jpeg");
            }

            return ValidationResult.Success;
        }
    }
}
