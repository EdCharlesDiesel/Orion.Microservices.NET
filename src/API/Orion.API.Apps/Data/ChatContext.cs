using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Entities;

namespace Orion.API.Chat.Data
{
    public class ChatRequestContext(DbContextOptions<ChatRequestContext> options) : DbContext(options)
    {
        public DbSet<ChatRequest> ChatRequests { get; set; }

        public async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
