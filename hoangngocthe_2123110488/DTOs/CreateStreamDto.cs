namespace hoangngocthe_2123110488.DTOs
{
    public class CreateStreamDto
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }
    }
}
