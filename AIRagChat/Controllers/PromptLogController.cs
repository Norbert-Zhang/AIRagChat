using AIRagChat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIRagChat.Controllers
{
    [ApiController]
    [Route("api/ai/v1/logs")]
    public class PromptLogController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PromptLogController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var logs = await _db.PromptLogs
                .OrderByDescending(x => x.Id)
                .Take(50)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
