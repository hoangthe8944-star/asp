using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Repository;

namespace hoangngocthe_2123110488.Service
{
    public interface IStreamService
    {
        Task<IEnumerable<StreamDto>> GetLiveStreamsAsync();
        Task<StreamDto?> GetByIdAsync(int id);
        Task<StreamDto> CreateAsync(int streamerId, CreateStreamRequest request);
        Task<StreamDto> UpdateAsync(int streamId, int streamerId, UpdateStreamRequest request);
        Task<StreamDto> StartStreamAsync(int streamId, int streamerId);
        Task<StreamDto> StopStreamAsync(int streamId, int streamerId);
        Task DeleteAsync(int streamId, int requesterId, string requesterRole);
        Task<IEnumerable<StreamDto>> SearchAsync(string? keyword, int? categoryId);
        Task<IEnumerable<StreamDto>> GetByStreamerAsync(int streamerId);
        Task<string> GetStreamKeyAsync(int userId);
        Task<string> ResetStreamKeyAsync(int userId);
        Task<StreamDto> StartByStreamKeyAsync(string key); // Hàm quan trọng nhất
        Task<StreamDto> StopByStreamKeyAsync(string key);
    }

    public class StreamService : IStreamService
    {
        private readonly IStreamRepository _streamRepo;
        private readonly IUserRepository _userRepo;

        public StreamService(IStreamRepository streamRepo, IUserRepository userRepo)
        {
            _streamRepo = streamRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<StreamDto>> GetLiveStreamsAsync()
        {
            var streams = await _streamRepo.GetLiveStreamsAsync();
            return streams.Select(MapToDto);
        }

        public async Task<StreamDto?> GetByIdAsync(int id)
        {
            var stream = await _streamRepo.GetWithDetailsAsync(id);
            return stream == null ? null : MapToDto(stream);
        }

        public async Task<StreamDto> CreateAsync(int streamerId, CreateStreamRequest request)
        {
            var stream = new Model.Stream
            {
                StreamerId = streamerId,
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Thumbnail = request.Thumbnail,
                StreamKey = Guid.NewGuid().ToString("N"),
                Status = "offline",
                ViewersCount = 0,
            };

            await _streamRepo.AddAsync(stream);
            return MapToDto(stream);
        }

        public async Task<StreamDto> UpdateAsync(int streamId, int streamerId, UpdateStreamRequest request)
        {
            var stream = await _streamRepo.GetByIdAsync(streamId)
                ?? throw new Exception("Không tìm thấy Stream.");

            if (stream.StreamerId != streamerId)
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa Stream này.");

            if (request.Title != null) stream.Title = request.Title;
            if (request.Description != null) stream.Description = request.Description;
            if (request.CategoryId != null) stream.CategoryId = request.CategoryId;
            if (request.Thumbnail != null) stream.Thumbnail = request.Thumbnail;

            await _streamRepo.UpdateAsync(stream);
            return MapToDto(stream);
        }

        public async Task<StreamDto> StartStreamAsync(int streamId, int streamerId)
        {
            var stream = await _streamRepo.GetByIdAsync(streamId)
                ?? throw new Exception("Không tìm thấy Stream.");

            if (stream.StreamerId != streamerId)
                throw new UnauthorizedAccessException("Không có quyền.");

            stream.Status = "live";
            stream.StartedAt = DateTime.UtcNow;
            stream.EndedAt = null;

            await _streamRepo.UpdateAsync(stream);
            return MapToDto(stream);
        }

        public async Task<StreamDto> StopStreamAsync(int streamId, int streamerId)
        {
            var stream = await _streamRepo.GetByIdAsync(streamId)
                ?? throw new Exception("Không tìm thấy Stream.");

            if (stream.StreamerId != streamerId)
                throw new UnauthorizedAccessException("Không có quyền.");

            stream.Status = "offline";
            stream.EndedAt = DateTime.UtcNow;

            await _streamRepo.UpdateAsync(stream);
            return MapToDto(stream);
        }

        public async Task DeleteAsync(int streamId, int requesterId, string requesterRole)
        {
            var stream = await _streamRepo.GetByIdAsync(streamId)
                ?? throw new Exception("Không tìm thấy Stream.");

            if (stream.StreamerId != requesterId && requesterRole != "admin")
                throw new UnauthorizedAccessException("Không có quyền.");

            await _streamRepo.DeleteAsync(stream);
        }

        public async Task<IEnumerable<StreamDto>> SearchAsync(string? keyword, int? categoryId)
        {
            var streams = await _streamRepo.SearchAsync(keyword ?? "", categoryId);
            return streams.Select(MapToDto);
        }

        public async Task<IEnumerable<StreamDto>> GetByStreamerAsync(int streamerId)
        {
            var streams = await _streamRepo.GetByStreamerIdAsync(streamerId);
            return streams.Select(MapToDto);
        }

        public async Task<string> GetStreamKeyAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            return user?.StreamKey ?? throw new Exception("Người dùng không tồn tại.");
        }

        public async Task<string> ResetStreamKeyAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new Exception("Người dùng không tồn tại.");

            user.StreamKey = Guid.NewGuid().ToString("N");
            await _userRepo.UpdateAsync(user);
            return user.StreamKey;
        }

        // ========================================================
        // 🚀 TỰ ĐỘNG NHẬN LUỒNG (BRIDGE LOGIC)
        // ========================================================
        public async Task<StreamDto> StartByStreamKeyAsync(string key)
        {
            // 1. Kiểm tra xem mã Key này thuộc về User nào trong hệ thống
            // Bạn cần đảm bảo IUserRepository có hàm GetByStreamKeyAsync
            var user = await _userRepo.GetByStreamKeyAsync(key)
                ?? throw new Exception("Stream Key không tồn tại trên hệ thống.");

            // 2. Tìm xem User này đã từng tạo Stream nào chưa
            var stream = await _streamRepo.GetByStreamKeyAsync(key);

            if (stream == null)
            {
                // Nếu User có Key đúng nhưng chưa bao giờ tạo luồng trong bảng Streams
                // Tự động tạo một bản ghi mới để Web hiển thị
                stream = new Model.Stream
                {
                    StreamerId = user.Id,
                    Title = $"{user.Username} is Streaming",
                    Description = "Livestream tự động kết nối qua Tool",
                    StreamKey = key,
                    Status = "live",
                    StartedAt = DateTime.UtcNow,
                    ViewersCount = 0
                };
                await _streamRepo.AddAsync(stream);
            }
            else
            {
                // Nếu đã có bản ghi, cập nhật trạng thái sang live
                stream.Status = "live";
                stream.StartedAt = DateTime.UtcNow;
                stream.EndedAt = null;
                await _streamRepo.UpdateAsync(stream);
            }

            return MapToDto(stream);
        }

        public async Task<StreamDto> StopByStreamKeyAsync(string key)
        {
            var stream = await _streamRepo.GetByStreamKeyAsync(key);
            if (stream != null)
            {
                stream.Status = "offline";
                stream.EndedAt = DateTime.UtcNow;
                await _streamRepo.UpdateAsync(stream);
                return MapToDto(stream);
            }
            throw new Exception("Không tìm thấy luồng để dừng.");
        }

        private static StreamDto MapToDto(Model.Stream s) => new()
        {
            Id = s.Id,
            StreamerId = s.StreamerId,
            StreamerName = s.Streamer?.Username ?? "Anonymous",
            Title = s.Title,
            Description = s.Description,
            CategoryId = s.CategoryId,
            CategoryName = s.Category?.Name,
            Thumbnail = s.Thumbnail,
            StreamKey = s.StreamKey,
            Status = s.Status,
            ViewersCount = s.ViewersCount,
            StartedAt = s.StartedAt,
            Tags = s.StreamTagMappings?.Select(m => m.Tag.Name).ToList() ?? new List<string>()
        };
    }
}