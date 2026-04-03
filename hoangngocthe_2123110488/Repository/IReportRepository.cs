using hoangngocthe_2123110488.Model;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllAsync();
    Task<Report> GetByIdAsync(int id);
    Task AddAsync(Report report);
    Task UpdateAsync(Report report);
    Task SaveChangesAsync();
}