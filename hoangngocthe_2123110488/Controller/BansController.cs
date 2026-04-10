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
    [Authorize(Roles = "admin")]
    public class BansController : ControllerBase
    {
        private readonly IBanService _svc;
        public BansController(IBanService svc) => _svc = svc;

        private int Uid => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// GET /api/bans?activeOnly=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false)
            => Ok(await _svc.GetAllAsync(activeOnly));

        /// GET /api/bans/user/:userId
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var ban = await _svc.GetByUserIdAsync(userId);
            return ban == null ? NotFound() : Ok(ban);
        }

        /// GET /api/bans/check/:userId
        [HttpGet("check/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckBan(int userId)
            => Ok(new { isBanned = await _svc.IsUserBannedAsync(userId) });

        /// POST /api/bans
        [HttpPost]
        public async Task<IActionResult> Ban([FromBody] CreateBanRequest request)
        {
            try { return Ok(await _svc.BanUserAsync(Uid, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// DELETE /api/bans/user/:userId  (unban)
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> Unban(int userId)
        {
            try { await _svc.UnbanUserAsync(userId); return Ok(new { message = "User unbanned." }); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}