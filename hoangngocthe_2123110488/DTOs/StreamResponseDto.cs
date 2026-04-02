namespace hoangngocthe_2123110488.DTOs
{
    public class StreamResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StreamKey { get; set; }
        public string CategoryName { get; set; }
        public List<string> Tags { get; set; }
        public string Status { get; set; }
    }
}
