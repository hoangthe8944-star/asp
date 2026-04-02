namespace hoangngocthe_2123110488.DTOs
{
    public class StreamDto
    {
        public int Id { get; set; }
        public int StreamerId { get; set; }
        public string StreamerName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Thumbnail { get; set; }
        public string StreamKey { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int ViewersCount { get; set; }
        public DateTime? StartedAt { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
