// using Microsoft.AspNetCore.SignalR;
// using Orion.API.ChatApi.Services;
// using Orion.DataAccess.Postgres.Entities;
//
// namespace Orion.API.ChatApi.Hubs;
//
// public class ChatHub(IChatService chatService) : Hub
// {
//
//     public async Task JoinRoom(string roomId, string username)
//     {
//         var room = chatService.GetRoom(roomId);
//         if (room is null)
//         {
//             await Clients.Caller.SendAsync("Error", $"Room '{roomId}' not found.");
//             return;
//         }
//
//         chatService.AddUser(Context.ConnectionId, username, roomId);
//         await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
//         var systemMsg = chatService.SaveMessage(
//             roomId, "System", $"{username} joined the room.", MessageType.System);
//
//         await Clients.Group(roomId).SendAsync("UserJoined", new
//         {
//             username,
//             users   = chatService.GetUsersInRoom(roomId),
//             message = systemMsg
//         });
//
//         var history = chatService.GetRoomHistory(roomId);
//         await Clients.Caller.SendAsync("RoomHistory", history);
//     }
//
//     public async Task SendMessage(string content)
//     {
//         var user = chatService.GetUser(Context.ConnectionId);
//         if (user is null)
//         {
//             await Clients.Caller.SendAsync("Error", "You must join a room first.");
//             return;
//         }
//
//         if (string.IsNullOrWhiteSpace(content))
//         {
//             await Clients.Caller.SendAsync("Error", "Message cannot be empty.");
//             return;
//         }
//
//         var message = chatService.SaveMessage(user.RoomId, user.Username, content);
//
//         // Broadcast to everyone in the room (including sender)
//         await Clients.Group(user.RoomId).SendAsync("ReceiveMessage", message);
//     }
//
//     // ── Typing indicator ─────────────────────────────────────────────────────
//
//     public async Task Typing(bool isTyping)
//     {
//         var user = chatService.GetUser(Context.ConnectionId);
//         if (user is null) return;
//
//         // Notify others in the room (not the sender)
//         await Clients.OthersInGroup(user.RoomId).SendAsync("UserTyping", new
//         {
//             user.Username,
//             isTyping
//         });
//     }
//
//     // ── Leave room ───────────────────────────────────────────────────────────
//
//     public async Task LeaveRoom()
//     {
//         var user = chatService.GetUser(Context.ConnectionId);
//         if (user is null) return;
//
//         await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.RoomId);
//         chatService.RemoveUser(Context.ConnectionId);
//
//         var systemMsg = chatService.SaveMessage(
//             user.RoomId, "System", $"{user.Username} left the room.", MessageType.System);
//
//         await Clients.Group(user.RoomId).SendAsync("UserLeft", new
//         {
//             user.Username,
//             users   = chatService.GetUsersInRoom(user.RoomId),
//             message = systemMsg
//         });
//     }
//
//     // ── Disconnect ────────────────────────────────────────────────────────────
//
//     public override async Task OnDisconnectedAsync(Exception? exception)
//     {
//         await LeaveRoom();
//         await base.OnDisconnectedAsync(exception);
//     }
// }