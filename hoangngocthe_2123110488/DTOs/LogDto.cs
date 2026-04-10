namespace hoangngocthe_2123110488.DTOs
{
    public class LogDto
    {
        public int Id { get; set; }
        public string Action { get; set; } = null!;
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? IpAddress { get; set; }
        public string? Detail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
