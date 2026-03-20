using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int StreamerId { get; set; }

        [Required]
        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }
    }
}
