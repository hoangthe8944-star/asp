using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

public interface ICategoryRepository
{
    Task<IEnumerable<StreamCategory>> GetAllAsync();
    Task<StreamCategory?> GetByIdAsync(int id);
    Task AddAsync(StreamCategory category);
    void Update(StreamCategory category);
    void Delete(StreamCategory category);
    Task<bool> SaveChangesAsync();
    Task<bool> ExistsAsync(string name);
}
public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StreamCategory>> GetAllAsync()
        => await _context.StreamCategories.Include(c => c.Streams).ToListAsync();

    public async Task<StreamCategory?> GetByIdAsync(int id)
        => await _context.StreamCategories.Include(c => c.Streams).FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(StreamCategory category) => await _context.StreamCategories.AddAsync(category);
    public void Update(StreamCategory category) => _context.StreamCategories.Update(category);
    public void Delete(StreamCategory category) => _context.StreamCategories.Remove(category);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    public async Task<bool> ExistsAsync(string name) => await _context.StreamCategories.AnyAsync(c => c.Name == name);
}