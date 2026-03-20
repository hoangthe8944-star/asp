using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class Stream
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StreamerId { get; set; }

        [ForeignKey("StreamerId")]
        public User Streamer { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public StreamCategory Category { get; set; }

        public string? Thumbnail { get; set; }

        [Required]
        public string StreamKey { get; set; }

        [Required]
        public string Status { get; set; } = "offline";

        public int ViewersCount { get; set; } = 0;

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        // Navigation
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
