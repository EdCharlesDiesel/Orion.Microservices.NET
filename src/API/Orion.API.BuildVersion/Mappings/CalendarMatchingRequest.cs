namespace Orion.API.BuildVersion.Mappings;

public class StringMeetingDto
{
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
}

public class CalendarMatchingRequest
{
    public List<StringMeetingDto> Calendar1 { get; set; } = new();
    public StringMeetingDto DailyBounds1 { get; set; } = new();
    public List<StringMeetingDto> Calendar2 { get; set; } = new();
    public StringMeetingDto DailyBounds2 { get; set; } = new();
    public int MeetingDuration { get; set; }
}

public class CalendarMatchingResponse
{
    public List<StringMeetingDto> AvailableSlots { get; set; } = new();
}