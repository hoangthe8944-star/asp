namespace hoangngocthe_2123110488.DTOs
{
    public class BanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public int BannedBy { get; set; }
        public string BannedByName { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateBanRequest
    {
        public int UserId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime? EndAt { get; set; }   // null = vĩnh viễn
    }
}
