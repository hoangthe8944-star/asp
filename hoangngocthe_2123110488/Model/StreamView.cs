using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class StreamView
    {
        [Key]
        public int Id { get; set; }

        public int StreamId { get; set; }
        public int UserId { get; set; }

        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
    }
}
