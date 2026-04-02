namespace hoangngocthe_2123110488.DTOs
{
    public class CreateDonationRequest
    {
        public int StreamerId { get; set; }
        public int? StreamId { get; set; }
        public decimal Amount { get; set; }
        public string? Message { get; set; }
    }
}
