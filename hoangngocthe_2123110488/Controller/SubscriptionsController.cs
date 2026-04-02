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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subService;
        public SubscriptionsController(ISubscriptionService subService) => _subService = subService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] CreateSubscriptionRequest request)
        {
            try { return Ok(await _subService.SubscribeAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("{streamerId}/status")]
        public async Task<IActionResult> Status(int streamerId)
            => Ok(await _subService.IsSubscribedAsync(CurrentUserId, streamerId));

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
            => Ok(await _subService.GetByUserAsync(CurrentUserId));
    }
}
