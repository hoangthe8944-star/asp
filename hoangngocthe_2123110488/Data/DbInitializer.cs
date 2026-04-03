using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Cần cài package BCrypt.Net-Next

namespace hoangngocthe_2123110488.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Đảm bảo Database đã được tạo
            context.Database.EnsureCreated();

            // Kiểm tra xem đã có tài khoản admin chưa
            if (context.Users.Any(u => u.Username == "admin" || u.Role == "admin"))
            {
                return; // Nếu có rồi thì thoát, không tạo trùng
            }

            // Tạo tài khoản Admin mới dựa trên Model của bạn
            var admin = new User
            {
                Username = "admin",
                Email = "admin@gmail.com",
                // Lưu ý: Password nên được Hash để bảo mật
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Avatar = "https://ui-avatars.com/api/?name=Admin&background=random",
                Bio = "Hệ thống quản trị viên",
                Role = "admin",
                Status = "active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.Users.Add(admin);

            try
            {
                context.SaveChanges();
                Console.WriteLine("--> Đã tạo tài khoản Admin mặc định thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Lỗi khi tạo Admin: {ex.Message}");
            }
        }
    }
}