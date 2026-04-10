using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Repository
{
    public interface IBanRepository
    {
        Task<IEnumerable<Ban>> GetAllAsync(bool activeOnly);
        Task<Ban?> GetLatestByUserIdAsync(int userId);
        Task<IEnumerable<Ban>> GetActiveBansByUserIdAsync(int userId);
        Task<bool> AnyActiveBanAsync(int userId);
        Task AddAsync(Ban ban);
        Task UpdateAsync(Ban ban);
        Task SaveChangesAsync();
    }
    public class BanRepository : IBanRepository
    {
        private readonly AppDbContext _db;
        public BanRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Ban>> GetAllAsync(bool activeOnly)
        {
            var query = _db.Bans
                .Include(b => b.User)
                .Include(b => b.BannedByUser) // Nạp dữ liệu BannedByUser
                .AsQueryable();

            if (activeOnly)
                query = query.Where(b => b.EndAt == null || b.EndAt > DateTime.UtcNow);

            return await query.OrderByDescending(b => b.StartAt).ToListAsync();
        }

        public async Task<Ban?> GetLatestByUserIdAsync(int userId)
        {
            return await _db.Bans
                .Include(b => b.User)
                .Include(b => b.BannedByUser)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ban>> GetActiveBansByUserIdAsync(int userId)
        {
            return await _db.Bans
                .Where(b => b.UserId == userId && (b.EndAt == null || b.EndAt > DateTime.UtcNow))
                .ToListAsync();
        }

        public async Task<bool> AnyActiveBanAsync(int userId)
        {
            return await _db.Bans.AnyAsync(b =>
                b.UserId == userId &&
                (b.EndAt == null || b.EndAt > DateTime.UtcNow));
        }

        public async Task AddAsync(Ban ban)
        {
            await _db.Bans.AddAsync(ban);
        }

        public async Task UpdateAsync(Ban ban)
        {
            _db.Bans.Update(ban);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}