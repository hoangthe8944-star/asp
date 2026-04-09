using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class BlacklistKeyword
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Word { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}