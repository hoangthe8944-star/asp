using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public class FollowRepository : GenericRepository<Follow>, IFollowRepository
    {
        public FollowRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsFollowingAsync(int followerId, int followingId)
            => await _dbSet.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

        public async Task<IEnumerable<Follow>> GetFollowersAsync(int userId)
            => await _dbSet.Include(f => f.FollowerId)
                .Where(f => f.FollowingId == userId).ToListAsync();

        public async Task<IEnumerable<Follow>> GetFollowingAsync(int userId)
            => await _dbSet.Include(f => f.FollowingId)
                .Where(f => f.FollowerId == userId).ToListAsync();

        public async Task<int> GetFollowerCountAsync(int userId)
            => await _dbSet.CountAsync(f => f.FollowingId == userId);
    }
}
