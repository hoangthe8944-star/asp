using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int StreamCount { get; set; } // Số lượng stream thuộc danh mục này
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}