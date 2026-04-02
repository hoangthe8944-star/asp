namespace hoangngocthe_2123110488.DTOs
{
    public class CreateReportRequest
    {
        public int TargetId { get; set; }
        public string TargetType { get; set; } = null!;
        public string Reason { get; set; } = null!;
    }
}
