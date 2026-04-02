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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role)!;

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userService.GetByIdAsync(CurrentUserId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
            => Ok(await _userService.GetAllAsync());

        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            try { return Ok(await _userService.UpdateAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{id}/ban")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Ban(int id, [FromBody] BanUserRequest request)
        {
            try { return Ok(await _userService.BanUserAsync(id, request.Reason, request.EndAt)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
