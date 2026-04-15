using AIRagChat.Interfaces;

namespace AIRagChat.AgentTools
{
    public class OrderTool : IAgentTool
    {
        public string Name => "GetOrder";
        public string Description => "根据订单ID查询订单信息";

        public async Task<string> ExecuteAsync(string input)
        {
            // 这里接你的真实数据库 / API
            //return $"订单 {input} 状态：已发货";
            // 模拟异步操作以解决 CS1998 问题
            return await Task.FromResult($"订单 {input} 状态：已发货");
        }
    }
}
