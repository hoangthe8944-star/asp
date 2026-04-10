namespace hoangngocthe_2123110488.Controller
{
    using System.Security.Claims;
    using hoangngocthe_2123110488.Model;
    using hoangngocthe_2123110488.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _svc;
        public AnalyticsController(IAnalyticsService svc) => _svc = svc;

        private int Uid => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool Auth => User.Identity?.IsAuthenticated ?? false;

        /// POST /api/analytics/streams/:id/join  (gọi khi viewer vào xem)
        [HttpPost("streams/{streamId}/join")]
        [AllowAnonymous]
        public async Task<IActionResult> Join(int streamId)
        {
            await _svc.JoinStreamAsync(streamId, Auth ? Uid : null);
            return NoContent();
        }

        /// POST /api/analytics/streams/:id/leave  (gọi khi viewer rời)
        [HttpPost("streams/{streamId}/leave")]
        [AllowAnonymous]
        public async Task<IActionResult> Leave(int streamId)
        {
            await _svc.LeaveStreamAsync(streamId, Auth ? Uid : null);
            return NoContent();
        }

        /// GET /api/analytics/streams/:id  (thống kê 1 stream)
        [HttpGet("streams/{streamId}")]
        [Authorize]
        public async Task<IActionResult> StreamStats(int streamId)
        {
            var stats = await _svc.GetStreamStatsAsync(streamId);
            return stats == null ? NotFound() : Ok(stats);
        }

        /// GET /api/analytics/streamers/:id  (thống kê tổng của streamer)
        [HttpGet("streamers/{streamerId}")]
        [Authorize]
        public async Task<IActionResult> StreamerStats(int streamerId)
        {
            var stats = await _svc.GetStreamerAnalyticsAsync(streamerId);
            return stats == null ? NotFound() : Ok(stats);
        }

        /// POST /api/analytics/streams/:id/recalc  (admin: tính lại stats)
        [HttpPost("streams/{streamId}/recalc")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Recalc(int streamId)
        {
            await _svc.RecalcStatsAsync(streamId);
            return NoContent();
        }
    }
}