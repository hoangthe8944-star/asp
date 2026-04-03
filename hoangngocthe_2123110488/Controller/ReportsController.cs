using hoangngocthe_2123110488.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportsController(IReportService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _service.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReportDto dto)
        {
            var result = await _service.CreateReportAsync(dto);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReportStatusDto dto)
        {
            var success = await _service.UpdateStatusAsync(id, dto.Status);
            if (!success) return NotFound();
            return Ok(new { message = "Status updated" });
        }
    }
}