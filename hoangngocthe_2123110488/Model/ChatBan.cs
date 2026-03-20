using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class ChatBan
    {
        [Key]
        public int Id { get; set; }

        public int StreamId { get; set; }
        public int UserId { get; set; }

        public int BannedBy { get; set; }

        public string? Reason { get; set; }

        public DateTime? ExpiredAt { get; set; }
    }
}
