// using Microsoft.AspNetCore.Mvc;
// using Orion.API.ChatApi.Services;
//
// namespace Orion.API.ChatApi.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class RoomsController(IChatService chatService) : ControllerBase
// {
//     [HttpGet]
//     public IActionResult GetAll() =>
//         Ok(chatService.GetAllRooms().Select(r => new
//         {
//             r.Id,
//             r.Name,
//             r.CreatedAt,
//             UserCount = r.Users.Count
//         }));
//
//     [HttpGet("{id}")]
//     public IActionResult GetById(string id)
//     {
//         var room = chatService.GetRoom(id);
//         return room is null ? NotFound($"Room '{id}' not found.") : Ok(room);
//     }
//
//     [HttpPost]
//     public IActionResult Create([FromBody] CreateRoomRequest request)
//     {
//         if (string.IsNullOrWhiteSpace(request.Name))
//             return BadRequest("Room name is required.");
//
//         var room = chatService.CreateRoom(request.Name);
//         return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
//     }
//
//     [HttpGet("{id}/history")]
//     public IActionResult GetHistory(string id, [FromQuery] int limit = 50)
//     {
//         var room = chatService.GetRoom(id);
//         if (room is null) return NotFound();
//
//         return Ok(chatService.GetRoomHistory(id, limit));
//     }
//
//     [HttpGet("{id}/users")]
//     public IActionResult GetUsers(string id)
//     {
//         var room = chatService.GetRoom(id);
//         if (room is null) return NotFound();
//
//         return Ok(chatService.GetUsersInRoom(id));
//     }
//
//     [HttpDelete("{id}")]
//     public IActionResult Delete(string id)
//     {
//         var deleted = chatService.DeleteRoom(id);
//         return deleted ? NoContent() : NotFound();
//     }
// }
//
// public record CreateRoomRequest(string Name);