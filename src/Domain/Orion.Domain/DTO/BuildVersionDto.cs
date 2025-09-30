using System;

namespace Orion.Domain.DTO;

public class BuildVersionDto
{
    public string DatabaseVersion { get; init; } // nvarchar(25)

    public DateTime? VersionDate { get; init; } // datetime

    public DateTime? ModifiedDate { get; init; } // datetime
}