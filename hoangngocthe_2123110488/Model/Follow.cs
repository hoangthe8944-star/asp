using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        // 1. Người đi theo dõi
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public virtual User Follower { get; set; }

        // 2. Người được theo dõi (Streamer)
        public int FollowingId { get; set; }

        // QUAN TRỌNG: Thêm dòng này để sửa lỗi "does not contain a definition for Following"
        [ForeignKey("FollowingId")]
        public virtual User Following { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}