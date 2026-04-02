using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class DonationDto
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại trỏ tới người donate
        public int UserId { get; set; }

        // QUAN TRỌNG: Thêm dòng này để sửa lỗi "does not contain a definition for User"
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string Username { get; set; } = string.Empty;
        public int StreamerId { get; set; }

        // Bạn cũng nên có Navigation cho Streamer nếu cần lấy tên Streamer
        [ForeignKey("StreamerId")]
        public virtual User Streamer { get; set; }

        public int? StreamId { get; set; }
        public decimal Amount { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}