using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data; // Đảm bảo đúng namespace của AppDbContext

namespace hoangngocthe_2123110488.Service
{
    public interface IFollowService
    {
        Task<bool> FollowAsync(int followerId, int followingId);
        Task<bool> UnfollowAsync(int followerId, int followingId);
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<IEnumerable<object>> GetFollowingAsync(int userId);
        Task<int> GetFollowerCountAsync(int userId);
    }

    public class FollowService : IFollowService
    {
        private readonly AppDbContext _context;

        // CHỈ GIỮ LẠI MỘT HÀM KHỞI TẠO NÀY
        public FollowService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> FollowAsync(int followerId, int followingId)
        {
            if (followerId == followingId)
                throw new Exception("Bạn không thể tự theo dõi chính mình.");

            // Kiểm tra trùng lặp trước khi lưu
            var isExisted = await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (isExisted) return true;

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnfollowAsync(int followerId, int followingId)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
        {
            return await _context.Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<IEnumerable<object>> GetFollowingAsync(int userId)
        {
            return await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Include(f => f.Following) // Đảm bảo bảng Follow có quan hệ với User
                .Select(f => new {
                    f.FollowingId,
                    Username = f.Following.Username,
                    Avatar = f.Following.Avatar
                })
                .ToListAsync();
        }

        public async Task<int> GetFollowerCountAsync(int userId)
        {
            return await _context.Follows.CountAsync(f => f.FollowingId == userId);
        }
    }
}