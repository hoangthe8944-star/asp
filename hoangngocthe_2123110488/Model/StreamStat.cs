using System.ComponentModel.DataAnnotations;

namespace hoangngocthe_2123110488.Model
{
    public class StreamStat
    {
        [Key]
        public int Id { get; set; }

        public int StreamId { get; set; }

        public int PeakViewers { get; set; }
        public int AvgViewers { get; set; }
        public int TotalViews { get; set; }
    }
}
