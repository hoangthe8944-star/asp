using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository.StreamingApp.Repositories;
using Microsoft.EntityFrameworkCore;
using hoangngocthe_2123110488.Data;

namespace hoangngocthe_2123110488.Repository
{
    public interface IChatRepository : IGenericRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetByStreamAsync(int streamId, int take = 50);
        Task<bool> IsUserBannedAsync(int streamId, int userId);
        Task AddBanAsync(ChatBan ban);
    }

    public class ChatRepository : GenericRepository<ChatMessage>, IChatRepository
    {
        private readonly AppDbContext _context;
        public ChatRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ChatMessage>> GetByStreamAsync(int streamId, int take = 50)
            => await _dbSet
                .Include(c => c.User)
                .Where(c => c.StreamId == streamId)
                .OrderByDescending(c => c.CreatedAt)
                .Take(take)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();


        public async Task<bool> IsUserBannedAsync(int streamId, int userId)
            => await _context.ChatBans.AnyAsync(b =>
                b.StreamId == streamId &&
                b.UserId == userId &&
                (b.ExpiredAt == null || b.ExpiredAt > DateTime.UtcNow));
        public async Task AddBanAsync(ChatBan ban)
        {
            await _context.ChatBans.AddAsync(ban);
            await _context.SaveChangesAsync();
        }
    }
}
