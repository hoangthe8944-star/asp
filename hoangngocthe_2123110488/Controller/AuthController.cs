using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try { return Ok(await _authService.RegisterAsync(request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try { return Ok(await _authService.LoginAsync(request)); }
            catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
        }
    }
}
