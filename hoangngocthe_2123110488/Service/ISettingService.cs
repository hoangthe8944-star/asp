using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore; 

namespace hoangngocthe_2123110488.Service
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingDto>> GetAllAsync();
        Task<string?> GetValueAsync(string key);
        Task<SettingDto> UpsertAsync(string key, string value);
        Task DeleteAsync(string key);
    }

    public class SettingService : ISettingService
    {
        private readonly AppDbContext _db;
        public SettingService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<SettingDto>> GetAllAsync()
        {
            var settings = await _db.Settings.OrderBy(s => s.Key).ToListAsync();
            return settings.Select(s => new SettingDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value
            });
        }

        public async Task<string?> GetValueAsync(string key)
        {
            var s = await _db.Settings.FirstOrDefaultAsync(s => s.Key == key);
            return s?.Value;
        }

        public async Task<SettingDto> UpsertAsync(string key, string value)
        {
            var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                setting = new Setting { Key = key, Value = value };
                _db.Settings.Add(setting);
            }
            else
            {
                setting.Value = value;
            }
            await _db.SaveChangesAsync();
            return new SettingDto { Id = setting.Id, Key = setting.Key, Value = setting.Value };
        }

        public async Task DeleteAsync(string key)
        {
            var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Key == key)
                ?? throw new Exception($"Setting '{key}' not found.");
            _db.Settings.Remove(setting);
            await _db.SaveChangesAsync();
        }
    }
}
