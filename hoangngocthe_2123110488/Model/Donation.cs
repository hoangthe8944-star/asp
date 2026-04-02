using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int StreamerId { get; set; }

        public int? StreamId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual User User { get; set; }
    }
}
