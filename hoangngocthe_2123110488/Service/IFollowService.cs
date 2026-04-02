using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;

namespace hoangngocthe_2123110488.Service
{
    public interface IFollowService
    {
        Task<bool> FollowAsync(int followerId, int followingId);
        Task<bool> UnfollowAsync(int followerId, int followingId);
        Task<bool> IsFollowingAsync(int followerId, int followingId);
        Task<IEnumerable<FollowDto>> GetFollowingAsync(int userId);
        Task<int> GetFollowerCountAsync(int userId);
    }

    public class FollowService : IFollowService
    {
        private readonly IFollowRepository _followRepo;
        private readonly INotificationRepository _notifRepo;

        public FollowService(IFollowRepository followRepo, INotificationRepository notifRepo)
        {
            _followRepo = followRepo;
            _notifRepo = notifRepo;
        }

        public async Task<bool> FollowAsync(int followerId, int followingId)
        {
            if (followerId == followingId) throw new Exception("Cannot follow yourself.");
            if (await _followRepo.IsFollowingAsync(followerId, followingId)) return false;

            await _followRepo.AddAsync(new Follow
            {
                FollowerId = followerId,
                FollowingId = followingId,
                CreatedAt = DateTime.UtcNow
            });

            // Tạo notification cho streamer
            await _notifRepo.AddAsync(new Notification
            {
                UserId = followingId,
                Type = "follow",
                Message = $"Someone started following you.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            return true;
        }

        public async Task<bool> UnfollowAsync(int followerId, int followingId)
        {
            var follow = await _followRepo.FirstOrDefaultAsync(
                f => f.FollowerId == followerId && f.FollowingId == followingId);
            if (follow == null) return false;
            await _followRepo.DeleteAsync(follow);
            return true;
        }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
            => await _followRepo.IsFollowingAsync(followerId, followingId);

        public async Task<IEnumerable<FollowDto>> GetFollowingAsync(int userId)
        {
            var follows = await _followRepo.GetFollowingAsync(userId);
            return follows.Select(f => new FollowDto
            {
                Id = f.Id,
                FollowerId = f.FollowerId,
                FollowingId = f.FollowingId,
                FollowingUsername = f.Following?.Username ?? "",
                FollowingAvatar = f.Following?.Avatar,
                CreatedAt = f.CreatedAt
            });
        }

        public async Task<int> GetFollowerCountAsync(int userId)
            => await _followRepo.GetFollowerCountAsync(userId);
    }
}
