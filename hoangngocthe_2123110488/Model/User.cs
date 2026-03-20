using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Avatar { get; set; }

        [StringLength(255)]
        public string? Bio { get; set; }

        [Required]
        public string Role { get; set; } // viewer / streamer / admin

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Stream> Streams { get; set; }
    }
}
