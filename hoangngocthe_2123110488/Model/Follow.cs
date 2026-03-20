using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FollowerId { get; set; }

        [Required]
        public int FollowingId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
