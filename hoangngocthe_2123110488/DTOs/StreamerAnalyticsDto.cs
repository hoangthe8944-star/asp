namespace hoangngocthe_2123110488.DTOs
{
    public class StreamerAnalyticsDto
    {
        public int StreamerId { get; set; }
        public string StreamerName { get; set; } = null!;
        public int TotalStreams { get; set; }
        public int TotalViews { get; set; }
        public int TotalFollowers { get; set; }
        public decimal TotalDonations { get; set; }
        public int ActiveSubscribers { get; set; }
        public List<StreamStatsDto> RecentStreams { get; set; } = new();
    }
}
