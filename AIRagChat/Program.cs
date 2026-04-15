using AIRagChat.AgentTools;
using AIRagChat.Data;
using AIRagChat.Interfaces;
using AIRagChat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("AiDb"));

builder.Services.AddHttpClient<OpenAIEmbeddingProvider>();
builder.Services.AddSingleton<Kernel>(sp =>
{
    var builderKernel = Kernel.CreateBuilder();
    builderKernel.AddOpenAIChatCompletion(builder.Configuration["OpenAI:ChatModel"] ?? "gpt-5-mini", Environment.GetEnvironmentVariable("OpenAI_Key_TranslateService") ?? "");
    return builderKernel.Build();
});
builder.Services.AddScoped<RagService>();
builder.Services.AddScoped<IMemoryService, MemoryService>();
builder.Services.AddScoped<IAIProvider, OpenAIProvider>();
builder.Services.AddScoped<AiOrchestrator>();
builder.Services.AddScoped<PromptLogService>();

// 注册Agent Tools
builder.Services.AddScoped<IAgentTool, OrderTool>();
builder.Services.AddScoped<IAgentTool, InventoryTool>();
builder.Services.AddScoped<IAgentTool, UserTool>();
//// 工具自动注册（反射）
//var toolTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IAgentTool).IsAssignableFrom(t) && !t.IsInterface);
//foreach (var type in toolTypes)
//{
//    builder.Services.AddScoped(typeof(IAgentTool), type);
//}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
