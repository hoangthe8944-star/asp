using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; } // Ví dụ: Admin, Moderator, Streamer, Viewer
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
