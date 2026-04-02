using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription?> GetActiveAsync(int userId, int streamerId);
        Task<IEnumerable<Subscription>> GetByUserAsync(int userId);
        Task<IEnumerable<Subscription>> GetByStreamerAsync(int streamerId);
    }

    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context) : base(context) { }

        public async Task<Subscription?> GetActiveAsync(int userId, int streamerId)
            => await _dbSet.FirstOrDefaultAsync(s =>
                s.UserId == userId && s.StreamerId == streamerId &&
                s.Status == "active" && s.EndDate > DateTime.UtcNow);

        public async Task<IEnumerable<Subscription>> GetByUserAsync(int userId)
            => await _dbSet.Include(s => s.Plan)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartDate).ToListAsync();

        public async Task<IEnumerable<Subscription>> GetByStreamerAsync(int streamerId)
            => await _dbSet.Include(s => s.User).Include(s => s.Plan)
                .Where(s => s.StreamerId == streamerId)
                .OrderByDescending(s => s.StartDate).ToListAsync();
    }
}
