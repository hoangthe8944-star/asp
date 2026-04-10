using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using hoangngocthe_2123110488.Service;

namespace hoangngocthe_2123110488.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IStreamCategoryService _service;
        public CategoriesController(IStreamCategoryService service) => _service = service;

        /// <summary>Danh sách tất cả categories (sắp xếp theo viewer)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>Chi tiết 1 category kèm metadata game</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _service.GetByIdAsync(id);
            return cat == null ? NotFound() : Ok(cat);
        }

        /// <summary>Tạo category thủ công</summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            try { return Ok(await _service.CreateAsync(request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>
        /// Sync category từ Steam theo AppID
        /// Ví dụ: POST /api/categories/sync-steam/730  → CS2
        ///        POST /api/categories/sync-steam/570  → Dota 2
        ///        POST /api/categories/sync-steam/1172470 → Apex Legends
        /// </summary>
        [HttpPost("sync-steam/{steamAppId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SyncFromSteam(int steamAppId)
        {
            try { return Ok(await _service.SyncFromSteamAsync(steamAppId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        /// <summary>Cập nhật viewer count + active streams cho category</summary>
        [HttpPost("{id}/update-stats")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStats(int id)
        {
            var result = await _service.UpdateStatsAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
        // Thêm vào CategoriesController.cs
        /// <summary>
        /// Tìm kiếm game trên Steam theo tên
        /// GET /api/categories/search-steam?q=valorant
        /// </summary>
        [HttpGet("search-steam")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SearchSteam([FromQuery] string q)
        {
            var http = HttpContext.RequestServices
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient();

            var url = $"https://store.steampowered.com/api/storesearch/?term={Uri.EscapeDataString(q)}&l=english&cc=US";
            var json = await http.GetStringAsync(url);

            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var items = doc.RootElement
                .GetProperty("items")
                .EnumerateArray()
                .Take(10)
                .Select(item => new
                {
                    appId = item.GetProperty("id").GetInt32(),
                    name = item.GetProperty("name").GetString(),
                    // tiny   = item.GetProperty("tiny_image").GetString(),
                    price = item.TryGetProperty("price", out var p)
                                    ? p.TryGetProperty("final", out var f) ? f.GetInt32() / 100m : 0
                                    : 0,
                    isFree = item.TryGetProperty("price", out var p2)
                                    ? false : true,
                    steamUrl = $"https://store.steampowered.com/app/{item.GetProperty("id").GetInt32()}"
                })
                .ToList();

            return Ok(items);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success) return NotFound(new { message = "Không tìm thấy danh mục" });

                return Ok(new { message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi 400 kèm tin nhắn giải thích (ví dụ: lỗi ràng buộc dữ liệu)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}