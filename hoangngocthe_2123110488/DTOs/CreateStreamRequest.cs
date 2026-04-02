namespace hoangngocthe_2123110488.DTOs
{
    public class CreateStreamRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? Thumbnail { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}
