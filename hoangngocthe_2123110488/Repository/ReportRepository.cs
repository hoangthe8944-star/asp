using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;
    public ReportRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Report>> GetAllAsync() => await _context.Reports.OrderByDescending(r => r.CreatedAt).ToListAsync();
    public async Task<Report> GetByIdAsync(int id) => await _context.Reports.FindAsync(id);
    public async Task AddAsync(Report report) => await _context.Reports.AddAsync(report);
    public async Task UpdateAsync(Report report) => _context.Reports.Update(report);
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}