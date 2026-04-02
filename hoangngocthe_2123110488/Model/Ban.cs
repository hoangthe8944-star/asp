namespace hoangngocthe_2123110488.Model
{
    public class Ban
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Người bị ban
        public int AdminId { get; set; } // Người thực hiện ban
        public string Reason { get; set; }
        public DateTime? ExpiresAt { get; set; } // Null nếu ban vĩnh viễn
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}
