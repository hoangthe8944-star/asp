namespace hoangngocthe_2123110488.DTOs
{
    public class BanUserRequest
    {
        public int UserId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime? EndAt { get; set; }
    }
}
