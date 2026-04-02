namespace hoangngocthe_2123110488.Model
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; } // Người báo cáo
        public int? ReportedUserId { get; set; } // Người bị báo cáo
        public int? StreamId { get; set; } // Stream bị báo cáo (nếu có)
        public string Reason { get; set; }
        public string Status { get; set; } // "pending", "resolved", "rejected"
        public DateTime CreatedAt { get; set; }
    }
}
