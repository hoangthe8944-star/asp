using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Service
{
    public interface ILogService
    {
        Task WriteAsync(string action, int? userId = null, string? detail = null, string? ip = null);
        Task<IEnumerable<LogDto>> GetLogsAsync(string? action = null, int? userId = null, int page = 1, int pageSize = 50);
        Task ClearOldLogsAsync(int daysOld = 30);
    }

    public class LogService : ILogService
    {
        private readonly AppDbContext _db;
        public LogService(AppDbContext db) => _db = db;

        public async Task WriteAsync(string action, int? userId = null, string? detail = null, string? ip = null)
        {
            _db.Logs.Add(new Log
            {
                Level = "Info", // or another appropriate value
                Message = action,
                ActionBy = userId?.ToString() ?? "System",
                Exception = detail ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogDto>> GetLogsAsync(
            string? action = null, int? userId = null, int page = 1, int pageSize = 50)
        {
            var q = _db.Logs.AsQueryable();
            if (!string.IsNullOrEmpty(action)) q = q.Where(l => l.Message.Contains(action));
            if (userId.HasValue) q = q.Where(l => l.ActionBy == userId.Value.ToString());

            var logs = await q
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return logs.Select(l => new LogDto
            {
                Id = l.Id,
                Action = l.Message,
                UserId = int.TryParse(l.ActionBy, out var uid) ? uid : (int?)null,
                Username = null, // Username cannot be resolved from Log
                Detail = l.Exception,
                CreatedAt = l.CreatedAt
            });
        }

        public async Task ClearOldLogsAsync(int daysOld = 30)
        {
            var cutoff = DateTime.UtcNow.AddDays(-daysOld);
            var old = await _db.Logs.Where(l => l.CreatedAt < cutoff).ToListAsync();
            _db.Logs.RemoveRange(old);
            await _db.SaveChangesAsync();
        }
    }
}
