using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

public interface IBanService
{
    Task<IEnumerable<BanDto>> GetAllAsync(bool activeOnly = false);
    Task<BanDto?> GetByUserIdAsync(int userId);
    Task<bool> IsUserBannedAsync(int userId);
    Task<BanDto> BanUserAsync(int adminId, CreateBanRequest request);
    Task UnbanUserAsync(int userId);
}

public class BanService : IBanService
{
    private readonly AppDbContext _db;
    public BanService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<BanDto>> GetAllAsync(bool activeOnly = false)
    {
        var q = _db.Bans
            .Include(b => b.User)
            .Include(b => b.BannedByUser)
            .AsQueryable();

        if (activeOnly)
            q = q.Where(b => b.EndAt == null || b.EndAt > DateTime.UtcNow);

        var list = await q.OrderByDescending(b => b.StartAt).ToListAsync();
        return list.Select(Map);
    }

    public async Task<BanDto?> GetByUserIdAsync(int userId)
    {
        var ban = await _db.Bans
            .Include(b => b.User)
            .Include(b => b.BannedByUser)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartAt)
            .FirstOrDefaultAsync();
        return ban == null ? null : Map(ban);
    }

    public async Task<bool> IsUserBannedAsync(int userId)
        => await _db.Bans.AnyAsync(b =>
            b.UserId == userId &&
            (b.EndAt == null || b.EndAt > DateTime.UtcNow));

    public async Task<BanDto> BanUserAsync(int adminId, CreateBanRequest request)
    {
        var user = await _db.Users.FindAsync(request.UserId)
            ?? throw new Exception("User not found.");

        if (user.Role == "admin")
            throw new Exception("Cannot ban an admin.");

        if (await IsUserBannedAsync(request.UserId))
            throw new Exception("User is already banned.");

        // Cập nhật status trong bảng Users
        user.Status = "banned";
        user.UpdatedAt = DateTime.UtcNow;

        var ban = new Ban
        {
            UserId = request.UserId,
            BannedById = adminId,
            Reason = request.Reason,
            StartAt = DateTime.UtcNow,
            EndAt = request.EndAt
        };
        _db.Bans.Add(ban);
        await _db.SaveChangesAsync();

        await _db.Entry(ban).Reference(b => b.User).LoadAsync();
        await _db.Entry(ban).Reference(b => b.BannedByUser).LoadAsync();
        return Map(ban);
    }

    public async Task UnbanUserAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId)
            ?? throw new Exception("User not found.");

        // Xóa tất cả ban records còn hiệu lực
        var activeBans = await _db.Bans
            .Where(b => b.UserId == userId && (b.EndAt == null || b.EndAt > DateTime.UtcNow))
            .ToListAsync();

        // Đặt EndAt = now để vô hiệu hoá
        activeBans.ForEach(b => b.EndAt = DateTime.UtcNow);

        user.Status = "active";
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private static BanDto Map(Ban b) => new()
    {
        Id = b.Id,
        UserId = b.UserId,
        Username = b.User?.Username ?? "",
        BannedBy = b.BannedById,
        BannedByName = b.BannedByUser?.Username ?? "",
        Reason = b.Reason,
        StartAt = b.StartAt,
        EndAt = b.EndAt,
        IsActive = b.EndAt == null || b.EndAt > DateTime.UtcNow
    };
}
