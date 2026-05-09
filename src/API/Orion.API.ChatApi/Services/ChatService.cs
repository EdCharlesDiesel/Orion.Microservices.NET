// using System.Collections.Concurrent;
// using Orion.API.ChatApi.Models;
// using Orion.DataAccess.Postgres.Entities;
//
// namespace Orion.API.ChatApi.Services;
//
// public class ChatService : IChatService
// {
//     private readonly ConcurrentDictionary<string, ChatRoom> _rooms = new();
//     private readonly ConcurrentDictionary<string, User>     _users = new();
//     public ChatRoom CreateRoom(string name)
//     {
//         var room = new ChatRoom { Name = name };
//         _rooms[room.Id] = room;
//         return room;
//     }
//
//     public ChatRoom? GetRoom(string roomId) =>
//         _rooms.TryGetValue(roomId, out var room) ? room : null;
//
//     public IEnumerable<ChatRoom> GetAllRooms() => _rooms.Values;
//     public bool DeleteRoom(string roomId) => _rooms.TryRemove(roomId, out _);
//     public User AddUser(string connectionId, string username, string roomId)
//     {
//         var user = new User
//         {
//             ConnectionId = connectionId,
//             Username     = username,
//             RoomId       = roomId
//         };
//
//         _users[connectionId] = user;
//
//         if (_rooms.TryGetValue(roomId, out var room))
//         {
//             lock (room.Users) room.Users.Add(user);
//         }
//
//         return user;
//     }
//
//     public User? GetUser(string connectionId) =>
//         _users.TryGetValue(connectionId, out var user) ? user : null;
//
//     public IEnumerable<User> GetUsersInRoom(string roomId) =>
//         _rooms.TryGetValue(roomId, out var room) ? room.Users : [];
//
//     public void RemoveUser(string connectionId)
//     {
//         if (!_users.TryRemove(connectionId, out var user)) return;
//
//         if (_rooms.TryGetValue(user.RoomId, out var room))
//         {
//             lock (room.Users) room.Users.RemoveAll(u => u.ConnectionId == connectionId);
//         }
//     }
//
//     public Message SaveMessage(string roomId, string sender, string content,
//                                MessageType type = MessageType.Text)
//     {
//         var message = new Message
//         {
//             RoomId  = roomId,
//             Sender  = sender,
//             Content = content,
//             Type    = type
//         };
//
//         if (_rooms.TryGetValue(roomId, out var room))
//         {
//             lock (room.History) room.History.Add(message);
//         }
//
//         return message;
//     }
//
//     public IEnumerable<Message> GetRoomHistory(string roomId, int limit = 50)
//     {
//         if (!_rooms.TryGetValue(roomId, out var room)) return [];
//
//         lock (room.History)
//             return room.History.TakeLast(limit).ToList();
//     }
// }