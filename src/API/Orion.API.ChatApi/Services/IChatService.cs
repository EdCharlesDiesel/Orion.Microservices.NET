// using Orion.API.ChatApi.Models;
// using Orion.DataAccess.Postgres.Entities;
//
// namespace Orion.API.ChatApi.Services;
//
// public interface IChatService
// {
//     // Rooms
//     ChatRoom  CreateRoom(string name);
//     ChatRoom? GetRoom(string roomId);
//     IEnumerable<ChatRoom> GetAllRooms();
//     bool DeleteRoom(string roomId);
//
//     // Users
//     User  AddUser(string connectionId, string username, string roomId);
//     User? GetUser(string connectionId);
//     IEnumerable<User> GetUsersInRoom(string roomId);
//     void RemoveUser(string connectionId);
//
//     // Messages
//     Message SaveMessage(string roomId, string sender, string content, MessageType type = MessageType.Text);
//     IEnumerable<Message> GetRoomHistory(string roomId, int limit = 50);
// }