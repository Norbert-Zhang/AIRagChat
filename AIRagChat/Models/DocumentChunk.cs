namespace AIRagChat.Models
{
    public class DocumentChunk
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string EmbeddingJson { get; set; } = string.Empty; // 向量（JSON存储）
        public string Source { get; set; }  = string.Empty; // 可选：文档来源（PDF名/模块名
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
