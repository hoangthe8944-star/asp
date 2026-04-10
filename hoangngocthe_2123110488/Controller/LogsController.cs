using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _svc;
        public LogsController(ILogService svc) => _svc = svc;

        /// GET /api/logs?action=login&userId=1&page=1&pageSize=50
        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] string? action = null,
            [FromQuery] int? userId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
            => Ok(await _svc.GetLogsAsync(action, userId, page, pageSize));

        /// DELETE /api/logs/clear?daysOld=30
        [HttpDelete("clear")]
        public async Task<IActionResult> Clear([FromQuery] int daysOld = 30)
        {
            await _svc.ClearOldLogsAsync(daysOld);
            return Ok(new { message = $"Logs older than {daysOld} days cleared." });
        }
    }
}
