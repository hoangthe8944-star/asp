namespace hoangngocthe_2123110488.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int? ReportedUserId { get; set; }
        public int? StreamId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReportDto
    {
        public int ReporterId { get; set; }
        public int? ReportedUserId { get; set; }
        public int? StreamId { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateReportStatusDto
    {
        public string Status { get; set; } // "resolved", "rejected"
    }
}