namespace Orion.DataAccess.Postgres.Entities;

public class ChatRoom
{
    public string       Id        { get; set; } = Guid.NewGuid().ToString();
    public string       Name      { get; set; } = string.Empty;
    public DateTime     CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ChatUser>   Users     { get; set; } = [];
    public List<Message> History  { get; set; } = [];
}