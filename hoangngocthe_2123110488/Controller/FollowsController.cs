using System.Security.Claims;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FollowsController : ControllerBase
    {
        private readonly IFollowService _followService;
        public FollowsController(IFollowService followService) => _followService = followService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("{followingId}")]
        public async Task<IActionResult> Follow(int followingId)
        {
            try { return Ok(await _followService.FollowAsync(CurrentUserId, followingId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{followingId}")]
        public async Task<IActionResult> Unfollow(int followingId)
            => Ok(await _followService.UnfollowAsync(CurrentUserId, followingId));

        [HttpGet("{followingId}/status")]
        public async Task<IActionResult> Status(int followingId)
            => Ok(await _followService.IsFollowingAsync(CurrentUserId, followingId));

        [HttpGet("following")]
        public async Task<IActionResult> GetFollowing()
            => Ok(await _followService.GetFollowingAsync(CurrentUserId));

        [HttpGet("{userId}/count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowerCount(int userId)
            => Ok(await _followService.GetFollowerCountAsync(userId));
    }
}
