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
        Task<IEnumerable<BlacklistKeyword>> GetAllKeywordsAsync();
        Task AddKeywordAsync(BlacklistKeyword keyword);
        Task DeleteKeywordAsync(int id);
    }

    public class ChatRepository : GenericRepository<ChatMessage>, IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context) : base(context) { _context = context; }

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
        public async Task<IEnumerable<BlacklistKeyword>> GetAllKeywordsAsync()
        {
            return await _context.BlacklistKeywords
                .OrderByDescending(k => k.AddedAt)
                .ToListAsync();
        }

        public async Task AddKeywordAsync(BlacklistKeyword keyword)
        {
            await _context.BlacklistKeywords.AddAsync(keyword);
            await _context.SaveChangesAsync();
        }

        public async Task<BlacklistKeyword?> GetKeywordByIdAsync(int id)
        {
            return await _context.BlacklistKeywords.FindAsync(id);
        }

        public async Task DeleteKeywordAsync(int id)
        {
            var keyword = await _context.BlacklistKeywords.FindAsync(id);
            if (keyword != null)
            {
                _context.BlacklistKeywords.Remove(keyword);
                await _context.SaveChangesAsync();
            }
        }
    }
}
