using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class StreamCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Stream> Streams { get; set; }
    }
}
