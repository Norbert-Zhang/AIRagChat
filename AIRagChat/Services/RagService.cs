using System.Text.Json;
using AIRagChat.Data;
using AIRagChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIRagChat.Services
{
    public class RagService
    {
        private readonly AppDbContext _db;
        private readonly OpenAIEmbeddingProvider _embedding;

        public RagService(AppDbContext db, OpenAIEmbeddingProvider embedding)
        {
            _db = db;
            _embedding = embedding;
        }

        // ================================
        // 1️⃣ 文档导入（RAG入口）
        // ================================
        public async Task IngestAsync(string text, string source = "default")
        {
            var chunks = SplitText(text);

            foreach (var chunk in chunks)
            {
                var vector = await _embedding.GenerateAsync(chunk);

                var entity = new DocumentChunk
                {
                    Content = chunk,
                    EmbeddingJson = _embedding.ToJson(vector),
                    Source = source,
                    CreatedAt = DateTime.UtcNow
                };

                _db.DocumentChunks.Add(entity);
            }

            await _db.SaveChangesAsync();
        }

        // ================================
        // 2️⃣ 相似度搜索（核心）
        // ================================
        public async Task<List<string>> SearchAsync(string query, int topK = 3)
        {
            var queryVector = await _embedding.GenerateAsync(query);

            var allChunks = await _db.DocumentChunks.ToListAsync();

            var results = allChunks
                .Select(chunk => new
                {
                    Content = chunk.Content,
                    Score = CosineSimilarity(queryVector, _embedding.FromJson(chunk.EmbeddingJson))
                })
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .Select(x => x.Content)
                .ToList();

            return results;
        }

        // ================================
        // 3️⃣ 文本切分（带优化）
        // ================================
        private List<string> SplitText(string text, int chunkSize = 300, int overlap = 50)
        {
            var chunks = new List<string>();

            int start = 0;

            while (start < text.Length)
            {
                var length = Math.Min(chunkSize, text.Length - start);

                var chunk = text.Substring(start, length);

                chunks.Add(chunk);

                start += chunkSize - overlap; // ⭐ 重叠
            }

            return chunks;
        }

        // ================================
        // 4️⃣ 余弦相似度
        // ================================
        private double CosineSimilarity(float[] v1, float[] v2)
        {
            double dot = 0, norm1 = 0, norm2 = 0;

            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                norm1 += v1[i] * v1[i];
                norm2 += v2[i] * v2[i];
            }

            if (norm1 != 0 && norm2 != 0)
            {
                return dot / (Math.Sqrt(norm1) * Math.Sqrt(norm2));
            }
            else
            {
                return 0;
            }
        }
    }
}
