using AIRagChat.Interfaces;
using AIRagChat.Models;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace AIRagChat.Services
{
    public class OpenAIProvider : IAIProvider
    {
        private readonly IChatCompletionService _chatService;
        private readonly ILogger<OpenAIProvider> _logger;

        public OpenAIProvider(Kernel kernel, ILogger<OpenAIProvider> logger)
        {
            _chatService = kernel.GetRequiredService<IChatCompletionService>();
            _logger = logger;
        }

        public async Task<string> ChatAsync(string userId, List<Message> messages)
        {
            var history = new ChatHistory();

            // 👉 把你的Message转换成SK格式
            foreach (var msg in messages)
            {
                switch (msg.Role.ToLower())
                {
                    case "system":
                        history.AddSystemMessage(msg.Content);
                        break;

                    case "user":
                        history.AddUserMessage(msg.Content);
                        break;

                    case "assistant":
                        history.AddAssistantMessage(msg.Content);
                        break;
                }
            }
            var response = string.Empty;
            try
            {
                var result = await _chatService.GetChatMessageContentAsync(history);
                response = result.Content ?? "";
                _logger.LogInformation("AI Prompt: {@Messages}", messages);
                _logger.LogInformation("AI Response: {Response}", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI 'GetChatMessageContentAsync' Calling Fails | User:{UserId}", userId);
                //throw new Exception("AI 'GetChatMessageContentAsync' Calling Fails", ex);
            }
            return response;
        }

        public async IAsyncEnumerable<string> ChatStreamAsync(string userId, List<Message> messages)
        {
            var history = new ChatHistory();
            // 👉 把你的Message转换成SK格式
            foreach (var msg in messages)
            {
                switch (msg.Role.ToLower())
                {
                    case "system":
                        history.AddSystemMessage(msg.Content);
                        break;

                    case "user":
                        history.AddUserMessage(msg.Content);
                        break;

                    case "assistant":
                        history.AddAssistantMessage(msg.Content);
                        break;
                }
            }
            _logger.LogInformation("Streaming started");
            // ⭐ 关键：流式调用
            await foreach (var chunk in _chatService.GetStreamingChatMessageContentsAsync(history))
            {
                var content = chunk.Content;
                _logger.LogDebug("Chunk: {Chunk}", content);
                if (!string.IsNullOrEmpty(content))
                {
                    yield return content;
                }
            }
        }
    }
}
