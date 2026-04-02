using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class StreamTag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<StreamTagMapping> StreamTagMappings { get; set; }
    }
}
