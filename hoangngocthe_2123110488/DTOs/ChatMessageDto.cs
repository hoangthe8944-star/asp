namespace hoangngocthe_2123110488.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int StreamId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Message { get; set; } = null!;
        public string Type { get; set; } = "text";
        public DateTime CreatedAt { get; set; }
    }
}
