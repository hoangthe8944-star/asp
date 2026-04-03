using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;

namespace hoangngocthe_2123110488.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StreamCount = c.Streams?.Count ?? 0
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description, StreamCount = c.Streams?.Count ?? 0 };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            if (await _repo.ExistsAsync(dto.Name))
                throw new Exception("Tên danh mục đã tồn tại");

            var category = new StreamCategory { Name = dto.Name, Description = dto.Description };
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            return new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            category.Name = dto.Name;
            category.Description = dto.Description;

            _repo.Update(category);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            // Kiểm tra nếu có stream thuộc danh mục này thì không cho xóa (tùy nghiệp vụ)
            if (category.Streams?.Any() == true)
                throw new Exception("Không thể xóa danh mục đang có Stream");

            _repo.Delete(category);
            return await _repo.SaveChangesAsync();
        }
    }
}
