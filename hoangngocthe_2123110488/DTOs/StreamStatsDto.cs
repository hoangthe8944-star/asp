namespace hoangngocthe_2123110488.DTOs
{
    public class StreamStatsDto
    {
        public int StreamId { get; set; }
        public string StreamTitle { get; set; } = null!;
        public int PeakViewers { get; set; }
        public double AvgViewers { get; set; }
        public int TotalViews { get; set; }
        public int UniqueViewers { get; set; }
        public double AvgWatchMinutes { get; set; }
    }
}
