namespace hoangngocthe_2123110488.Controller
{
    using System.Security.Claims;
    using hoangngocthe_2123110488.DTOs;
    using hoangngocthe_2123110488.Model;
    using hoangngocthe_2123110488.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _svc;
        public VideosController(IVideoService svc) => _svc = svc;

        private int Uid => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";

        /// GET /api/videos/streamer/:streamerId  (VOD list)
        [HttpGet("streamer/{streamerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStreamer(int streamerId)
            => Ok(await _svc.GetByStreamerAsync(streamerId));

        /// GET /api/videos/:id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var v = await _svc.GetByIdAsync(id);
            return v == null ? NotFound() : Ok(v);
        }

        /// POST /api/videos  (streamer đăng VOD)
        [HttpPost]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Create([FromBody] CreateVideoRequest request)
        {
            try { return Ok(await _svc.CreateAsync(Uid, request)); }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// POST /api/videos/:id/view  (tăng view khi xem VOD)
        [HttpPost("{id}/view")]
        [AllowAnonymous]
        public async Task<IActionResult> View(int id)
        {
            await _svc.IncrementViewAsync(id);
            return NoContent();
        }

        /// DELETE /api/videos/:id
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _svc.DeleteAsync(id, Uid, Role);
                return NoContent();
            }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}