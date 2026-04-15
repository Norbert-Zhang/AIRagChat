using AIRagChat.Models;
using AIRagChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIRagChat.Controllers
{
    [ApiController]
    [Route("api/ai/v1")]
    public class AiController : ControllerBase
    {
        private readonly AiOrchestrator _ai;
        private readonly RagService _rag;

        public AiController(AiOrchestrator ai, RagService rag)
        {
            _ai = ai;
            _rag = rag;
        }

        // 👉 导入文档
        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody] string text)
        {
            await _rag.IngestAsync(text);
            return Ok("ok");
        }

        // 👉 提问
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req)
        {
            var result = await _ai.AskAsync(req.userId, req.Message);
            return Ok(result);
        }
    }
}
