using AIRagChat.Interfaces;
using AIRagChat.Models;

namespace AIRagChat.Services
{
    public class AiOrchestrator
    {
        private readonly IAIProvider _ai;
        private readonly IMemoryService _memory;
        private readonly RagService _rag;
        private readonly IEnumerable<IAgentTool> _tools;
        private readonly PromptLogService _log;

        public AiOrchestrator(
            IAIProvider ai,
            IMemoryService memory,
            RagService rag,
            IEnumerable<IAgentTool> tools,
            PromptLogService log)
        {
            _ai = ai;
            _memory = memory;
            _rag = rag;
            _tools = tools;
            _log = log;
        }

        public async Task<string> AskAsync(string userId, string question)
        {
            // 1️⃣ 记忆
            var history = await _memory.GetHistoryAsync(userId);

            // 2️⃣ RAG
            var docs = await _rag.SearchAsync(question);
            var context = string.Join("\n", docs);

            // 3️⃣ Agent提示
            var toolDesc = string.Join("\n", _tools.Select(t =>
                $"{t.Name}: {t.Description}"));

            var systemPrompt = $@"你是企业AI助手，你可以使用工具：{toolDesc} 知识库内容： {context} 规则：- 优先使用知识库 - 如果需要调用工具, 只能使用上述工具名, 不确定就不要调用工具，调用格式： CALL_TOOL: 工具名|参数";

            var messages = new List<Message>
            {
                new Message { Role = "system", Content = systemPrompt }
            };
            messages.AddRange(history);
            messages.Add(new Message { Role = "user", Content = question });

            // 👉 拼完整Prompt（用于日志）
            var fullPrompt = string.Join("\n", messages.Select(m => $"{m.Role}: {m.Content}"));

            var response = await _ai.ChatAsync(userId, messages);

            // 4️⃣ Agent执行
            if (response.StartsWith("CALL_TOOL:"))
            {
                var parts = response.Replace("CALL_TOOL:", "").Split('|');
                var toolName = parts[0];
                var input = parts.Length > 1 ? parts[1] : "";

                var tool = _tools.FirstOrDefault(t => t.Name == toolName);

                if (tool != null)
                {
                    var toolResult = await tool.ExecuteAsync(input);
                    return await _ai.ChatAsync(userId, new List<Message>
                    {
                        new Message { Role = "system", Content = "根据工具结果回答用户" },
                        new Message { Role = "user", Content = question },
                        new Message { Role = "assistant", Content = toolResult }
                
                    });
                }
            }

            // ⭐⭐⭐ 记录日志（关键）
            await _log.LogAsync(userId, fullPrompt, response);

            // 5️⃣ 保存记忆
            await _memory.SaveMessageAsync(userId, "user", question);
            await _memory.SaveMessageAsync(userId, "assistant", response);

            return response;
        }
    }
}
