using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreamController : ControllerBase
    {
        private readonly StreamService _streamService;

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] CreateStreamDto dto)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value); // Lấy ID từ Token
            var result = await _streamService.StartStream(userId, dto);
            return Ok(result);
        }

        [HttpGet("live")]
        public async Task<IActionResult> GetLive()
        {
            // Trả về danh sách các stream đang status == 'live'
        }
    }
}
