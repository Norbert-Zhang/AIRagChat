using AIRagChat.Interfaces;

namespace AIRagChat.AgentTools
{
    public class InventoryTool : IAgentTool
    {
        public string Name => "GetInventory";
        public string Description => "根据商品ID查询库存";

        public Task<string> ExecuteAsync(string input)
        {
            return Task.FromResult($"商品 {input} 库存：100件");
        }
    }
}
