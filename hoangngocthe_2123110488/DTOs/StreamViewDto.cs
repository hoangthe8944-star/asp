namespace hoangngocthe_2123110488.DTOs
{
    public class StreamViewDto
    {
        public int Id { get; set; }
        public int StreamId { get; set; }
        public int? UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public int WatchDurationSeconds { get; set; }
    }
}
