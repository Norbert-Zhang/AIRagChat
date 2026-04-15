using AIRagChat.Models;
using Microsoft.EntityFrameworkCore;

namespace AIRagChat.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DocumentChunk> DocumentChunks { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<PromptLog> PromptLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================
            // DocumentChunk 配置
            // ========================
            modelBuilder.Entity<DocumentChunk>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                    .IsRequired();

                entity.Property(e => e.EmbeddingJson)
                    .IsRequired();

                //entity.Property(e => e.Source)
                //    .HasMaxLength(200);

                //entity.Property(e => e.CreatedAt)
                //    .HasDefaultValueSql("GETUTCDATE()");
            });

            // ========================
            // ChatMessage 配置
            // ========================
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Content)
                    .IsRequired();

                //entity.Property(e => e.CreatedAt)
                //    .HasDefaultValueSql("GETUTCDATE()");

                // 👉 索引（非常重要）
                entity.HasIndex(e => e.UserId);
            });

            // ========================
            // PromptLog 配置
            // ========================
            modelBuilder.Entity<PromptLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .HasMaxLength(100);

                entity.Property(e => e.Prompt)
                    .IsRequired();

                entity.Property(e => e.Response)
                    .IsRequired();

                //entity.Property(e => e.CreatedAt)
                //    .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
