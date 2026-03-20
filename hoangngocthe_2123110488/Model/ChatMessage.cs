using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StreamId { get; set; }

        [ForeignKey("StreamId")]
        public Stream Stream { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required, StringLength(500)]
        public string Message { get; set; }

        [Required]
        public string Type { get; set; } = "text";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
