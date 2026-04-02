using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public interface IDonationRepository : IGenericRepository<Donation>
    {
        Task<IEnumerable<Donation>> GetByStreamAsync(int streamId);
        Task<IEnumerable<Donation>> GetByStreamerAsync(int streamerId);
        Task<decimal> GetTotalDonationAsync(int streamerId);
    }

    public class DonationRepository : GenericRepository<Donation>, IDonationRepository
    {
        public DonationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Donation>> GetByStreamerAsync(int streamerId)
            => await _dbSet.Include(d => d.UserId)
                .Where(d => d.StreamerId == streamerId)
                .OrderByDescending(d => d.CreatedAt).ToListAsync();

        public async Task<IEnumerable<Donation>> GetByStreamAsync(int streamId)
            => await _dbSet.Include(d => d.UserId)
                .Where(d => d.StreamId == streamId)
                .OrderByDescending(d => d.CreatedAt).ToListAsync();

        public async Task<decimal> GetTotalDonationAsync(int streamerId)
            => await _dbSet.Where(d => d.StreamerId == streamerId).SumAsync(d => d.Amount);
    }
}
