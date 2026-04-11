using System;

namespace Orion.Domain.DTO;

public class ErrorLogDto
{

        public int? ErrorLogID { get; set; } // int

        public DateTime? ErrorTime { get; set; } // datetime

        public string UserName { get; set; } // nvarchar(128)

        public int? ErrorNumber { get; set; } // int

        public int? ErrorSeverity { get; set; } // int

        public int? ErrorState { get; set; } // int

        public string ErrorProcedure { get; set; } // nvarchar(126)
 
        public int? ErrorLine { get; set; } // int
        public string ErrorMessage { get; set; } // nvarchar(4000)
}