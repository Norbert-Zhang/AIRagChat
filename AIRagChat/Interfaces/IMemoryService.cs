using AIRagChat.Models;

namespace AIRagChat.Interfaces
{
    public interface IMemoryService
    {
        Task<List<Message>> GetHistoryAsync(string userId);
        Task SaveMessageAsync(string userId, string role, string content);
    }
}
