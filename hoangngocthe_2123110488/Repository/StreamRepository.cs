using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public class StreamRepository : GenericRepository<Model.Stream>, IStreamRepository
    {
        public StreamRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Model.Stream>> GetLiveStreamsAsync()
            => await _dbSet
                .Include(s => s.StreamTagMappings).ThenInclude(m => m.Tag)
                .Include(s => s.Category)
                .Where(s => s.Status == "live")
                .OrderByDescending(s => s.ViewersCount)
                .ToListAsync();

        public async Task<IEnumerable<Model.Stream>> GetByStreamerIdAsync(int streamerId)
            => await _dbSet
                .Where(s => s.StreamerId == streamerId)
                .OrderByDescending(s => s.StartedAt)
                .ToListAsync();

        public async Task<Model.Stream?> GetWithDetailsAsync(int streamId)
            => await _dbSet
                .Include(s => s.Streamer)
                .Include(s => s.Category)
                .Include(s => s.StreamTagMappings).ThenInclude(m => m.Tag)
                .FirstOrDefaultAsync(s => s.Id == streamId);

        public async Task<IEnumerable<Model.Stream>> SearchAsync(string keyword, int? categoryId)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(s => s.Title.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(s => s.CategoryId == categoryId);
            return await query.OrderByDescending(s => s.ViewersCount).ToListAsync();
        }
        public async Task<Model.Stream?> GetByStreamKeyAsync(string streamKey)
        {
            return await _dbSet
                .Include(s => s.Streamer)
                .Include(s => s.Category)
                .Include(s => s.StreamTagMappings)
                    .ThenInclude(m => m.Tag)
                .FirstOrDefaultAsync(s => s.StreamKey == streamKey);
        }
    }
}
