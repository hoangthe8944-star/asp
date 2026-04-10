using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Service
{
    public interface IAnalyticsService
    {
        Task JoinStreamAsync(int streamId, int? userId);
        Task LeaveStreamAsync(int streamId, int? userId);
        Task<StreamStatsDto?> GetStreamStatsAsync(int streamId);
        Task<StreamerAnalyticsDto?> GetStreamerAnalyticsAsync(int streamerId);
        Task RecalcStatsAsync(int streamId);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly AppDbContext _db;
        public AnalyticsService(AppDbContext db) => _db = db;

        public async Task JoinStreamAsync(int streamId, int? userId)
        {
            // Tăng viewer count
            var stream = await _db.Streams.FindAsync(streamId);
            if (stream != null)
            {
                stream.ViewersCount++;
                await _db.SaveChangesAsync();
            }

            // Ghi nhận lượt xem
            _db.StreamViews.Add(new StreamView
            {
                StreamId = streamId,
                UserId = userId ?? 0,
                JoinedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        public async Task LeaveStreamAsync(int streamId, int? userId)
        {
            // Giảm viewer count
            var stream = await _db.Streams.FindAsync(streamId);
            if (stream != null && stream.ViewersCount > 0)
            {
                stream.ViewersCount--;
                await _db.SaveChangesAsync();
            }

            // Cập nhật thời gian rời
            var view = await _db.StreamViews
                .Where(v => v.StreamId == streamId && v.UserId == userId && v.LeftAt == null)
                .OrderByDescending(v => v.JoinedAt)
                .FirstOrDefaultAsync();

            if (view != null)
            {
                view.LeftAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<StreamStatsDto?> GetStreamStatsAsync(int streamId)
        {
            var stream = await _db.Streams.FindAsync(streamId);
            if (stream == null) return null;

            var stats = await _db.StreamStats
                .FirstOrDefaultAsync(s => s.StreamId == streamId);

            var views = await _db.StreamViews
                .Where(v => v.StreamId == streamId).ToListAsync();

            var avgWatch = views
                .Where(v => v.LeftAt.HasValue)
                .Select(v => (v.LeftAt!.Value - v.JoinedAt).TotalSeconds)
                .DefaultIfEmpty(0)
                .Average();

            return new StreamStatsDto
            {
                StreamId = streamId,
                StreamTitle = stream.Title,
                PeakViewers = stats?.PeakViewers ?? 0,
                AvgViewers = stats?.AvgViewers ?? 0,
                TotalViews = stats?.TotalViews ?? views.Count,
                UniqueViewers = views.Select(v => v.UserId).Distinct().Count(),
                AvgWatchMinutes = Math.Round(avgWatch / 60, 1)
            };
        }

        public async Task<StreamerAnalyticsDto?> GetStreamerAnalyticsAsync(int streamerId)
        {
            var user = await _db.Users.FindAsync(streamerId);
            if (user == null) return null;

            var streamIds = await _db.Streams
                .Where(s => s.StreamerId == streamerId)
                .Select(s => s.Id).ToListAsync();

            var totalViews = await _db.StreamViews
                .Where(v => streamIds.Contains(v.StreamId)).CountAsync();

            var totalFollowers = await _db.Follows
                .CountAsync(f => f.FollowingId == streamerId);

            var totalDonations = await _db.Donations
                .Where(d => d.StreamerId == streamerId).SumAsync(d => d.Amount);

            var activeSubscribers = await _db.Subscriptions
                .CountAsync(s => s.StreamerId == streamerId &&
                                 s.Status == "active" &&
                                 s.EndDate > DateTime.UtcNow);

            var recentStreamStats = await _db.StreamStats
                .Where(s => streamIds.Contains(s.StreamId))
                .Join(_db.Streams, ss => ss.StreamId, s => s.Id,
                    (ss, s) => new StreamStatsDto
                    {
                        StreamId = s.Id,
                        StreamTitle = s.Title,
                        PeakViewers = ss.PeakViewers,
                        AvgViewers = ss.AvgViewers,
                        TotalViews = ss.TotalViews,
                    })
                .Take(10)
                .ToListAsync();

            return new StreamerAnalyticsDto
            {
                StreamerId = streamerId,
                StreamerName = user.Username,
                TotalStreams = streamIds.Count,
                TotalViews = totalViews,
                TotalFollowers = totalFollowers,
                TotalDonations = totalDonations,
                ActiveSubscribers = activeSubscribers,
                RecentStreams = recentStreamStats
            };
        }

        public async Task RecalcStatsAsync(int streamId)
        {
            var views = await _db.StreamViews
                .Where(v => v.StreamId == streamId).ToListAsync();

            if (!views.Any()) return;

            // Tính peak viewers: nhóm theo phút
            var peak = views.Count;  // simplified

            var avgWatch = views
                .Where(v => v.LeftAt.HasValue)
                .Select(v => (v.LeftAt!.Value - v.JoinedAt).TotalSeconds)
                .DefaultIfEmpty(0).Average();

            var existing = await _db.StreamStats
                .FirstOrDefaultAsync(s => s.StreamId == streamId);

            if (existing == null)
            {
                _db.StreamStats.Add(new StreamStat
                {
                    StreamId = streamId,
                    PeakViewers = peak,
                    AvgViewers = views.Count > 0 ? (int)(views.Count / 2.0) : 0,
                    TotalViews = views.Count
                });
            }
            else
            {
                existing.PeakViewers = Math.Max(existing.PeakViewers, peak);
                existing.TotalViews = views.Count;
            }

            await _db.SaveChangesAsync();
        }
    }

}
