namespace hoangngocthe_2123110488.DTOs
{
    public class ChatBanRequest
    {
        public int StreamId { get; set; }
        public int UserId { get; set; }
        public string? Reason { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}
