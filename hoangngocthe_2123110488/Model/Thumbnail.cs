namespace hoangngocthe_2123110488.Model
{
    public class Thumbnail
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Size { get; set; } // "small", "medium", "large"
        public int? StreamId { get; set; }
        public int? VideoId { get; set; }
    }
}
