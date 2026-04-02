namespace hoangngocthe_2123110488.Model
{
    public class Video
    {
        public int Id { get; set; }
        public int StreamerId { get; set; }
        public int? StreamId { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public int Duration { get; set; } // Giây
        public long Views { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Streamer { get; set; }
    }
}
