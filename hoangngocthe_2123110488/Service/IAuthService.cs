using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.Repository;
using Microsoft.IdentityModel.Tokens;

namespace hoangngocthe_2123110488.Service
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepo.EmailExistsAsync(request.Email))
                throw new Exception("Email already exists.");
            if (await _userRepo.UsernameExistsAsync(request.Username))
                throw new Exception("Username already exists.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "viewer",
                Status = "active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StreamKey = Guid.NewGuid().ToString("N")
            };

            await _userRepo.AddAsync(user);
            return new AuthResponse
            {
                Token = GenerateJwt(user),
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email)
                ?? throw new Exception("Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new Exception("Invalid email or password.");

            if (user.Status == "banned")
                throw new Exception("Your account has been banned.");

            return new AuthResponse
            {
                Token = GenerateJwt(user),
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id
            };
        }

        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,            user.Username),
                new Claim(ClaimTypes.Email,           user.Email),
                new Claim(ClaimTypes.Role,            user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
