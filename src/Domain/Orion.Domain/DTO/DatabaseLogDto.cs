using System;

namespace Orion.Domain.DTO;

public abstract class DatabaseLogDto
{
    public DateTime? PostTime { get; set; } // datetime

    public string DatabaseUser { get; set; } // nvarchar(128)

    public string Event { get; set; } // nvarchar(128)

    public string Schema { get; set; } // nvarchar(128)

    public string Object { get; set; } // nvarchar(128)

    public string TSQL { get; set; } // nvarchar(max)

    public string XmlEvent { get; set; } // XML(.)
}