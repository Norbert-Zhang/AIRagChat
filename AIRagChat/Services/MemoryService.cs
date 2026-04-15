using AIRagChat.Data;
using AIRagChat.Interfaces;
using AIRagChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIRagChat.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly AppDbContext _db;

        public MemoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Message>> GetHistoryAsync(string userId)
        {
            return await _db.ChatMessages
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => new Message
                {
                    Role = x.Role,
                    Content = x.Content
                })
                .ToListAsync();
        }

        public async Task SaveMessageAsync(string userId, string role, string content)
        {
            _db.ChatMessages.Add(new ChatMessage
            {
                UserId = userId,
                Role = role,
                Content = content
            });

            await _db.SaveChangesAsync();
        }
    }

}
