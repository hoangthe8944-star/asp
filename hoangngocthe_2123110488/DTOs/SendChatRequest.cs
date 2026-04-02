namespace hoangngocthe_2123110488.DTOs
{
    public class SendChatRequest
    {
        public int StreamId { get; set; }
        public string Message { get; set; } = null!;
        public string Type { get; set; } = "text";
    }
}
