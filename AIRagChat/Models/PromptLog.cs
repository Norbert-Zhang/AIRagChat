namespace AIRagChat.Models
{
    public class PromptLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Prompt { get; set; } = string.Empty;

        public string Response { get; set; } = string.Empty;

        public int? Tokens { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
