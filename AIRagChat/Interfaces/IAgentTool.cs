namespace AIRagChat.Interfaces
{
    public interface IAgentTool
    {
        string Name { get; }
        string Description { get; }

        Task<string> ExecuteAsync(string input);
    }
}
