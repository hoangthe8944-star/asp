using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class Ban
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BannedById { get; set; } // ID người ban
        public string? Reason { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        // NAVIGATION PROPERTIES (Giúp hết lỗi BannedByUser)
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("BannedById")]
        public virtual User? BannedByUser { get; set; } // Thuộc tính này bị báo lỗi nếu thiếu
    }
}
