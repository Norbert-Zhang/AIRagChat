using AIRagChat.Models;

namespace AIRagChat.Interfaces
{
    public interface IAIProvider
    {
        Task<string> ChatAsync(string userId, List<Message> messages);

        // ⭐ 新增：流式输出
        IAsyncEnumerable<string> ChatStreamAsync(string userId, List<Message> messages);
    }
}
