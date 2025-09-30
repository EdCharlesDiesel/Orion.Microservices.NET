using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Entities;

namespace Orion.API.Chat.Data
{
    public interface IChatContext
    {
        DbSet<ChatRequest> ChatRequest { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
