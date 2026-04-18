namespace hoangngocthe_2123110488.DTOs
{
    public class StreamDto
    {
        public int Id { get; set; }
        public int StreamerId { get; set; }
        public string StreamerName { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Thumbnail { get; set; }
        public string StreamKey { get; set; } = "";
        public string Status { get; set; } = "offline";
        public int ViewersCount { get; set; }
        public DateTime? StartedAt { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    public class CreateStreamRequest
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? Thumbnail { get; set; }
    }

    public class UpdateStreamRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? Thumbnail { get; set; }
    }
}