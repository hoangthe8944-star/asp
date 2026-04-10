using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Service
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoDto>> GetByStreamerAsync(int streamerId);
        Task<VideoDto?> GetByIdAsync(int id);
        Task<VideoDto> CreateAsync(int streamerId, CreateVideoRequest request);
        Task DeleteAsync(int id, int requesterId, string role);
        Task IncrementViewAsync(int id);
    }

    public class VideoService : IVideoService
    {
        private readonly AppDbContext _db;
        public VideoService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<VideoDto>> GetByStreamerAsync(int streamerId)
        {
            var videos = await _db.Videos
                .Include(v => v.Streamer)
                .Where(v => v.StreamerId == streamerId)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
            return videos.Select(Map);
        }

        public async Task<VideoDto?> GetByIdAsync(int id)
        {
            var v = await _db.Videos
                .Include(v => v.Streamer)
                .FirstOrDefaultAsync(v => v.Id == id);
            return v == null ? null : Map(v);
        }

        public async Task<VideoDto> CreateAsync(int streamerId, CreateVideoRequest request)
        {
            // Check that the passed-in streamerId matches the method argument, not a property on request
            var video = new Video
            {
                StreamerId = streamerId,
                Title = request.Title,
                VideoUrl = request.Url,
                Duration = request.Duration,
                CreatedAt = DateTime.UtcNow
            };
            _db.Videos.Add(video);
            await _db.SaveChangesAsync();

            await _db.Entry(video).Reference(v => v.Streamer).LoadAsync();
            return Map(video);
        }

        public async Task DeleteAsync(int id, int requesterId, string role)
        {
            var video = await _db.Videos
                .Include(v => v.Streamer)
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new Exception("Video not found.");

            if (video.StreamerId != requesterId && role != "admin")
                throw new UnauthorizedAccessException("Access denied.");

            _db.Videos.Remove(video);
            await _db.SaveChangesAsync();
        }

        public async Task IncrementViewAsync(int id)
        {
            var video = await _db.Videos.FindAsync(id);
            if (video == null) return;
            video.Views++;
            await _db.SaveChangesAsync();
        }

        private static VideoDto Map(Video v) => new()
        {
            Id = v.Id,
            StreamId = v.StreamId ?? 0,
            StreamTitle = "", // No Stream, so leave blank or fetch if needed
            StreamerId = v.StreamerId,
            StreamerName = v.Streamer?.Username ?? "",
            Title = v.Title,
            Url = v.VideoUrl,
            Duration = v.Duration,
            ViewCount = (int)v.Views, // Explicit cast from long to int
            CreatedAt = v.CreatedAt
        };
    }
}
