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
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService) => _chatService = chatService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("{streamId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMessages(int streamId, [FromQuery] int take = 50)
            => Ok(await _chatService.GetMessagesAsync(streamId, take));

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendChatRequest request)
        {
            try { return Ok(await _chatService.SendMessageAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("ban")]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Ban([FromBody] ChatBanRequest request)
        {
            try
            {
                await _chatService.BanUserAsync(CurrentUserId, request);
                return Ok(new { message = "User banned from chat." });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
