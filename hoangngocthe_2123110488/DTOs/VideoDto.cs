namespace hoangngocthe_2123110488.DTOs
{
    public class VideoDto
    {
        public int Id { get; set; }
        public int StreamId { get; set; }
        public string StreamTitle { get; set; } = null!;
        public int StreamerId { get; set; }
        public string StreamerName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public int Duration { get; set; }   // seconds
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateVideoRequest
    {
        public int StreamId { get; set; }
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public int Duration { get; set; }
    }
}
