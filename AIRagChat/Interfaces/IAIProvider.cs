using AIRagChat.Models;

namespace AIRagChat.Interfaces
{
    public interface IAIProvider
    {
        Task<string> ChatAsync(string userId, List<Message> messages);
    }
}
