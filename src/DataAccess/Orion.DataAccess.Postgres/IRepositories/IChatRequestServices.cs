using Orion.Domain.IRepositories;

namespace Orion.DataAccess.Postgres.IRepositories;
public interface IChatRequestServices:IRepository<ChatRequest>
{
    Task<string> GetChatRequests();
}

public class ChatRequest
{
}