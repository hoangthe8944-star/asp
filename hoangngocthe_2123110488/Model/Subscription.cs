using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        // 1. Khóa ngoại và Navigation trỏ tới người mua (User)
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } // LỖI NẰM Ở ĐÂY: Phải có dòng này

        // 2. Khóa ngoại và Navigation trỏ tới Streamer được đăng ký
        public int StreamerId { get; set; }

        [ForeignKey("StreamerId")]
        public virtual User Streamer { get; set; }

        [Required]
        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }
        public int SubscriptionPlanId { get; set; }

        // QUAN TRỌNG: Đặt tên là Plan để khớp với code Service/Controller
        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlan Plan { get; set; }
    }
}
