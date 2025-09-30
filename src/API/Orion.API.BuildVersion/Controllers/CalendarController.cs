using Microsoft.AspNetCore.Mvc;
using Orion.API.BuildVersion.Mappings;
using Orion.Helpers.Arrays;

namespace Orion.API.BuildVersion.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    [HttpPost("match")]
    public ActionResult<CalendarMatchingResponse> MatchCalendars([FromBody] CalendarMatchingRequest request)
    {
        if (request == null || request.Calendar1 == null || request.Calendar2 == null)
            return BadRequest("Invalid input");

        var result = CalendarMatchingClass.CalendarMatching(
            request.Calendar1.Select(c => new StringMeeting(c.Start, c.End)).ToList(),
            new StringMeeting(request.DailyBounds1.Start, request.DailyBounds1.End),
            request.Calendar2.Select(c => new StringMeeting(c.Start, c.End)).ToList(),
            new StringMeeting(request.DailyBounds2.Start, request.DailyBounds2.End),
            request.MeetingDuration
        );

        return Ok(new CalendarMatchingResponse
        {
            AvailableSlots = result.Select(r => new StringMeetingDto { Start = r.Start, End = r.End }).ToList()
        });
    }
}