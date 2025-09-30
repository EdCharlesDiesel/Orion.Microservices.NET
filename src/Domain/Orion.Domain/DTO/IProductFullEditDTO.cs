﻿using System;

namespace Orion.Domain.DTOs
{
    public interface IProductFullEditDto
    {
        int Id { get; set; }
        
        string ProductName { get; set; }

        string Description { get; set;}

        string Image { get; set;}

        byte[] CoverImage {get;set;}

        decimal UnitPrice { get; set; }

        int DurationInDays { get; }

        DateTime? StartValidityDate { get; }

        DateTime? EndValidityDate { get; }

        int CategoryId { get; }
    }
}
