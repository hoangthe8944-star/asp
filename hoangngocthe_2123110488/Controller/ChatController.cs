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
        // GET: api/chat/blacklist
        [HttpGet("blacklist")]
        public async Task<IActionResult> GetBlacklist()
        {
            var keywords = await _chatService.GetAllKeywordsAsync();
            return Ok(keywords);
        }

        // POST: api/chat/blacklist
        [HttpPost("blacklist")]
        public async Task<IActionResult> AddBlacklist([FromBody] KeywordRequest request)
        {
            if (string.IsNullOrEmpty(request.Word)) return BadRequest("Word is required");

            var newKeyword = new BlacklistKeyword
            {
                Word = request.Word.ToLower().Trim(),
                AddedAt = DateTime.UtcNow
            };

            await _chatService.AddKeywordAsync(newKeyword);
            return Ok(newKeyword);
        }

        // DELETE: api/chat/blacklist/5
        [HttpDelete("blacklist/{id}")]
        public async Task<IActionResult> DeleteBlacklist(int id)
        {
            await _chatService.DeleteKeywordAsync(id);
            return Ok();
        }
    }

    // Class phụ để nhận dữ liệu từ React gửi lên
    public class KeywordRequest
    {
        public string Word { get; set; }
    }
}

