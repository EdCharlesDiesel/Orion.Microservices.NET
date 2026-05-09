namespace Orion.DataAccess.Postgres.Entities;


public class ChatUser
{
    public string ConnectionId { get; set; } = string.Empty;
    public string Username    { get; set; } = string.Empty;
    public string RoomId      { get; set; } = string.Empty;
    public DateTime JoinedAt  { get; set; } = DateTime.UtcNow;
}