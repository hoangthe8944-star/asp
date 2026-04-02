namespace hoangngocthe_2123110488.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
    }
}
