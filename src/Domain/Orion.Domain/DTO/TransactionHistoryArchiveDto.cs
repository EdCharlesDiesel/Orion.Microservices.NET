using System;

namespace Orion.Domain.DTO;

public class TransactionHistoryArchiveDto
{

    public int? TransactionID { get; set; } // int

    public int? ProductID { get; set; } // int

    public int? ReferenceOrderID { get; set; } // int

    public int? ReferenceOrderLineID { get; set; } // int

    public DateTime? TransactionDate { get; set; } // datetime

    public string TransactionType { get; set; } // nchar(1)

    public int? Quantity { get; set; } // int

    public decimal? ActualCost { get; set; } // money

    public DateTime? ModifiedDate { get; set; } // datetime
}