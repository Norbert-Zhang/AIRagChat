namespace AIRagChat.Models
{
    public class Message
    {
        public string Role { get; set; } = string.Empty; // system / user / assistant
        public string Content { get; set; } = string.Empty;
    }
}
