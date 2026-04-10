using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Service
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetByUserAsync(int userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkReadAsync(int notificationId, int userId);
        Task MarkAllReadAsync(int userId);
        Task DeleteAsync(int notificationId, int userId);
        Task<NotificationDto> CreateAsync(int userId, string type, string content);
        // Gửi thông báo "streamer đang live" đến toàn bộ follower
        Task NotifyFollowersStreamLiveAsync(int streamerId, string streamerName, int streamId);
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        public NotificationService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<NotificationDto>> GetByUserAsync(int userId, bool unreadOnly = false)
        {
            var q = _db.Notifications.Where(n => n.UserId == userId);
            if (unreadOnly) q = q.Where(n => !n.IsRead);
            var list = await q.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return list.Select(Map);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
            => await _db.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

        public async Task MarkReadAsync(int id, int userId)
        {
            var n = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId)
                ?? throw new Exception("Notification not found.");
            n.IsRead = true;
            await _db.SaveChangesAsync();
        }

        public async Task MarkAllReadAsync(int userId)
        {
            var list = await _db.Notifications
                .Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
            list.ForEach(n => n.IsRead = true);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var n = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId)
                ?? throw new Exception("Notification not found.");
            _db.Notifications.Remove(n);
            await _db.SaveChangesAsync();
        }

        public async Task<NotificationDto> CreateAsync(int userId, string type, string content)
        {
            var n = new Notification
            {
                UserId = userId,
                Type = type,
                Message = content,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();
            return Map(n);
        }

        public async Task NotifyFollowersStreamLiveAsync(int streamerId, string streamerName, int streamId)
        {
            // Lấy tất cả follower của streamer
            var followerIds = await _db.Follows
                .Where(f => f.FollowingId == streamerId)
                .Select(f => f.FollowerId)
                .ToListAsync();

            var notifications = followerIds.Select(fid => new Notification
            {
                UserId = fid,
                Type = "stream_live",
                Message = $"{streamerName} đang phát sóng trực tiếp!",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _db.Notifications.AddRangeAsync(notifications);
            await _db.SaveChangesAsync();
        }

        private static NotificationDto Map(Notification n) => new()
        {
            Id = n.Id,
            UserId = n.UserId,
            Type = n.Type,
            Content = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        };
    }
}
