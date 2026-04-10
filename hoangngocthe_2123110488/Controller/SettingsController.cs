using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _svc;
        public SettingsController(ISettingService svc) => _svc = svc;

        /// GET /api/settings
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _svc.GetAllAsync());

        /// GET /api/settings/:key
        [HttpGet("{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var val = await _svc.GetValueAsync(key);
            return val == null ? NotFound() : Ok(new { key, value = val });
        }

        /// PUT /api/settings  (tạo hoặc cập nhật)
        [HttpPut]
        public async Task<IActionResult> Upsert([FromBody] UpsertSettingRequest req)
        {
            try { return Ok(await _svc.UpsertAsync(req.Key, req.Value)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// DELETE /api/settings/:key
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            try { await _svc.DeleteAsync(key); return NoContent(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
