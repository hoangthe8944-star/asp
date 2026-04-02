using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Model;

namespace hoangngocthe_2123110488.Repository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserAsync(int userId, bool unreadOnly = false);
        Task MarkAllReadAsync(int userId);
    }

    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Notification>> GetByUserAsync(int userId, bool unreadOnly = false)
        {
            var query = _dbSet.Where(n => n.UserId == userId);
            if (unreadOnly) query = query.Where(n => !n.IsRead);
            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task MarkAllReadAsync(int userId)
        {
            var notifs = await _dbSet.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
            notifs.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();
        }
    }
}
