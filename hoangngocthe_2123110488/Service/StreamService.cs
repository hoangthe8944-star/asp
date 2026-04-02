using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;

namespace hoangngocthe_2123110488.Service
{
    public class StreamService
    {
        private readonly AppDbContext _context;
        public StreamService(AppDbContext context) { _context = context; }

        public async Task<StreamResponseDto> StartStream(int streamerId, CreateStreamDto dto)
        {
            var stream = new Stream
            {
                StreamerId = streamerId,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                StreamKey = Guid.NewGuid().ToString().Replace("-", ""), // Tạo Key ngẫu nhiên
                Status = "live",
                StartedAt = DateTime.UtcNow
            };

            _context.Streams.Add(stream);
            await _context.SaveChangesAsync();

            // Thêm Tags vào Mapping
            foreach (var tagId in dto.TagIds)
            {
                _context.StreamTagMappings.Add(new StreamTagMapping { StreamId = stream.Id, TagId = tagId });
            }
            await _context.SaveChangesAsync();

            return new StreamResponseDto { /* map data */ };
        }
    }
}
