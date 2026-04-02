using System.Security.Claims;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreamsController : ControllerBase
    {
        private readonly IStreamService _streamService;
        public StreamsController(IStreamService streamService) => _streamService = streamService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role) ?? "";

        [HttpGet("live")]
        public async Task<IActionResult> GetLive()
            => Ok(await _streamService.GetLiveStreamsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stream = await _streamService.GetByIdAsync(id);
            return stream == null ? NotFound() : Ok(stream);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int? categoryId)
            => Ok(await _streamService.SearchAsync(keyword, categoryId));

        [HttpGet("streamer/{streamerId}")]
        public async Task<IActionResult> GetByStreamer(int streamerId)
            => Ok(await _streamService.GetByStreamerAsync(streamerId));

        [HttpPost]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Create([FromBody] CreateStreamRequest request)
        {
            try { return Ok(await _streamService.CreateAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStreamRequest request)
        {
            try { return Ok(await _streamService.UpdateAsync(id, CurrentUserId, request)); }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{id}/start")]
        [Authorize(Roles = "streamer")]
        public async Task<IActionResult> Start(int id)
        {
            try { return Ok(await _streamService.StartStreamAsync(id, CurrentUserId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{id}/stop")]
        [Authorize(Roles = "streamer")]
        public async Task<IActionResult> Stop(int id)
        {
            try { return Ok(await _streamService.StopStreamAsync(id, CurrentUserId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _streamService.DeleteAsync(id, CurrentUserId, CurrentRole);
                return NoContent();
            }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
