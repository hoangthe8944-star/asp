using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;

namespace hoangngocthe_2123110488.Service
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> UpdateAsync(int userId, UpdateUserRequest request);
        Task<bool> BanUserAsync(int userId, string reason, DateTime? endAt);
        Task<IEnumerable<UserDto>> GetAllAsync();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> UpdateAsync(int userId, UpdateUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");

            if (request.Avatar != null) user.Avatar = request.Avatar;
            if (request.Bio != null) user.Bio = request.Bio;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<bool> BanUserAsync(int userId, string reason, DateTime? endAt)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");
            user.Status = "banned";
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);
            return true;
        }

        private static UserDto MapToDto(User u) => new()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Avatar = u.Avatar,
            Bio = u.Bio,
            Role = u.Role,
            Status = u.Status,
            CreatedAt = u.CreatedAt
        };
    }
}
