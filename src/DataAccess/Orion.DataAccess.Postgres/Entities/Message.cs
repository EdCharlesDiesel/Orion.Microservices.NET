namespace Orion.DataAccess.Postgres.Entities;

public class Message
{
    public Guid     Id        { get; set; } = Guid.NewGuid();
    public string   RoomId    { get; set; } = string.Empty;
    public string   Sender    { get; set; } = string.Empty;
    public string   Content   { get; set; } = string.Empty;
    public DateTime SentAt    { get; set; } = DateTime.UtcNow;
    public MessageType Type   { get; set; } = MessageType.Text;
}

public enum MessageType { Text, System, File }