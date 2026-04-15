using AIRagChat.Data;
using AIRagChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIRagChat.Services
{
    public class PromptLogService
    {
        private readonly AppDbContext _db;

        public PromptLogService(AppDbContext db)
        {
            _db = db;
        }

        public async Task LogAsync(
            string userId,
            string prompt,
            string response,
            int? tokens = null)
        {
            var log = new PromptLog
            {
                UserId = userId,
                Prompt = prompt,
                Response = response,
                Tokens = tokens,
                CreatedAt = DateTime.UtcNow
            };

            _db.PromptLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
