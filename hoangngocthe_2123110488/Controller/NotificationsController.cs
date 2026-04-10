namespace hoangngocthe_2123110488.Controller
{
    using hoangngocthe_2123110488.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _svc;
        public NotificationsController(INotificationService svc) => _svc = svc;

        private int Uid => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// GET /api/notifications
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool unreadOnly = false)
            => Ok(await _svc.GetByUserAsync(Uid, unreadOnly));

        /// GET /api/notifications/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> UnreadCount()
            => Ok(await _svc.GetUnreadCountAsync(Uid));

        /// PUT /api/notifications/:id/read
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            try { await _svc.MarkReadAsync(id, Uid); return NoContent(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// PUT /api/notifications/read-all
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            await _svc.MarkAllReadAsync(Uid);
            return NoContent();
        }

        /// DELETE /api/notifications/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _svc.DeleteAsync(id, Uid); return NoContent(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}