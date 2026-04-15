namespace AIRagChat.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty; // system / user / assistant

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
