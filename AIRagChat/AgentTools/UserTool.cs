using AIRagChat.Interfaces;

namespace AIRagChat.AgentTools
{
    public class UserTool : IAgentTool
    {
        public string Name => "GetUser";
        public string Description => "根据用户ID获取用户信息";

        public Task<string> ExecuteAsync(string input)
        {
            return Task.FromResult($"用户 {input}：VIP用户");
        }
    }
}
