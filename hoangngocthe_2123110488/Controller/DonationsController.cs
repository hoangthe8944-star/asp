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
    public class DonationsController : ControllerBase
    {
        private readonly IDonationService _donationService;
        public DonationsController(IDonationService donationService) => _donationService = donationService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Donate([FromBody] CreateDonationRequest request)
        {
            try { return Ok(await _donationService.DonateAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet("streamer/{streamerId}")]
        public async Task<IActionResult> GetByStreamer(int streamerId)
            => Ok(await _donationService.GetByStreamerAsync(streamerId));

        [HttpGet("streamer/{streamerId}/total")]
        public async Task<IActionResult> GetTotal(int streamerId)
            => Ok(await _donationService.GetTotalAsync(streamerId));
    }
}
