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
    public class StreamsController : ControllerBase
    {
        private readonly IStreamService _streamService;
        private readonly IWebHostEnvironment _env; // Thêm môi trường web để lấy đường dẫn wwwroot

        public StreamsController(IStreamService streamService, IWebHostEnvironment env)
        {
            _streamService = streamService;
            _env = env;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role) ?? "";

        [HttpGet("live")]
        public async Task<IActionResult> GetLive()
            => Ok(await _streamService.GetLiveStreamsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stream = await _streamService.GetByIdAsync(id);
            return stream == null ? NotFound() : Ok(stream);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int? categoryId)
            => Ok(await _streamService.SearchAsync(keyword, categoryId));

        [HttpGet("streamer/{streamerId}")]
        public async Task<IActionResult> GetByStreamer(int streamerId)
            => Ok(await _streamService.GetByStreamerAsync(streamerId));

        [HttpPost]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Create([FromBody] CreateStreamRequest request)
        {
            try { return Ok(await _streamService.CreateAsync(CurrentUserId, request)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "streamer,admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStreamRequest request)
        {
            try { return Ok(await _streamService.UpdateAsync(id, CurrentUserId, request)); }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{id}/start")]
        [Authorize(Roles = "streamer")]
        public async Task<IActionResult> Start(int id)
        {
            try { return Ok(await _streamService.StartStreamAsync(id, CurrentUserId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("{id}/stop")]
        [Authorize(Roles = "streamer")]
        public async Task<IActionResult> Stop(int id)
        {
            try { return Ok(await _streamService.StopStreamAsync(id, CurrentUserId)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _streamService.DeleteAsync(id, CurrentUserId, CurrentRole);
                return NoContent();
            }
            catch (UnauthorizedAccessException) { return Forbid(); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc thiếu userId");
            return int.Parse(userIdClaim.Value);
        }

        // =============================
        // 🔑 LẤY STREAM KEY
        // =============================
        [Authorize]
        [HttpGet("me/key")]
        public async Task<IActionResult> GetMyStreamKey()
        {
            try
            {
                var userId = GetUserId();
                var key = await _streamService.GetStreamKeyAsync(userId);
                return Ok(new { streamKey = key });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =============================
        // 🔄 RESET STREAM KEY
        // =============================
        [Authorize]
        [HttpPost("me/reset-key")]
        public async Task<IActionResult> ResetStreamKey()
        {
            try
            {
                var userId = GetUserId();
                var key = await _streamService.ResetStreamKeyAsync(userId);
                return Ok(new { streamKey = key });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("start-by-key")]
        public async Task<IActionResult> StartByKey([FromBody] StreamKeyRequest req)
        {
            var stream = await _streamService.StartByStreamKeyAsync(req.StreamKey);
            return Ok(stream);
        }

        [HttpPost("stop-by-key")]
        public async Task<IActionResult> StopByKey([FromBody] StreamKeyRequest req)
        {
            var stream = await _streamService.StopByStreamKeyAsync(req.StreamKey);
            return Ok(stream);
        }

        // =============================
        // 🖼️ UPLOAD THUMBNAIL
        // =============================
        [HttpPost("upload/thumbnail")]
        [Authorize]
        public async Task<IActionResult> UploadThumbnail(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Vui lòng chọn một file ảnh." });

            // Kiểm tra định dạng file ảnh cho an toàn
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Định dạng file không hỗ trợ. Chỉ nhận .jpg, .jpeg, .png, .webp" });

            try
            {
                // Đường dẫn thư mục: wwwroot/thumbnails
                var folderPath = Path.Combine(_env.WebRootPath, "thumbnails");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tạo tên file duy nhất bằng GUID
                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(folderPath, fileName);

                // Lưu file vào ổ đĩa
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về URL đầy đủ để Frontend có thể hiển thị ảnh ngay lập tức
                var url = $"{Request.Scheme}://{Request.Host}/thumbnails/{fileName}";
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lưu file: " + ex.Message });
            }
        }

        [HttpPost("webhook")]
        [AllowAnonymous] // Đảm bảo MediaMTX có thể gọi vào mà không cần Token
        public async Task<IActionResult> MediaMTXWebhook([FromBody] MediaMTXRequest data)
        {
            try
            {
                // Kiểm tra log để debug (tùy chọn)
                Console.WriteLine($"Webhook nhận: Action={data.Action}, Path={data.Path}");

                if (string.IsNullOrEmpty(data.Path)) return BadRequest();

                // Lấy mã Key từ Path (Ví dụ: "live/afb374..." -> "afb374...")
                string key = data.Path.Split('/').Last();

                if (data.Action == "publish")
                {
                    await _streamService.StartByStreamKeyAsync(key);
                    Console.WriteLine($"[SUCCESS] Luồng {key} đã bắt đầu Live.");
                }
                else if (data.Action == "user_disconnect")
                {
                    await _streamService.StopByStreamKeyAsync(key);
                    Console.WriteLine($"[SUCCESS] Luồng {key} đã dừng.");
                }

                return Ok(); // MediaMTX cần nhận 200 OK để cho phép stream tiếp
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Webhook lỗi: {ex.Message}");
                // Trả về 401 nếu không tìm thấy Key trong DB để MediaMTX ngắt kết nối FFmpeg
                return Unauthorized();
            }
        }
    }
}