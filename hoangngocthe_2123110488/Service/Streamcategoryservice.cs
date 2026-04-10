using System.Text.Json;
using System.Text.Json.Serialization;
using hoangngocthe_2123110488.Data;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Service
{
    // ══════════════════════════════════════════════════════
    //  DTOs
    // ══════════════════════════════════════════════════════
    public class StreamCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public string? TrailerVideoUrl { get; set; }
        public string? GameplayVideoUrl { get; set; }
        public List<string> Screenshots { get; set; } = new();
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<string> Platforms { get; set; } = new();
        public List<string> SteamTags { get; set; } = new();
        public int? MetacriticScore { get; set; }
        public int? SteamReviewScore { get; set; }
        public int? SteamReviewCount { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public bool IsFreeToPlay { get; set; }
        public string? SteamStoreUrl { get; set; }
        public string? OfficialWebsiteUrl { get; set; }
        public int CurrentViewers { get; set; }
        public int ActiveStreamsCount { get; set; }
        public int? PopularityRank { get; set; }
        public int? SteamAppId { get; set; }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? SteamAppId { get; set; }  // nếu có → tự sync từ Steam
        public string? RawgSlug { get; set; }  // nếu có → tự sync từ RAWG
        public string? CoverImageUrl { get; set; }
    }

    // ══════════════════════════════════════════════════════
    //  STEAM API RESPONSE MODELS (dùng nội bộ)
    // ══════════════════════════════════════════════════════
    internal class SteamAppDetailsResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("data")] public SteamAppData? Data { get; set; }
    }

    internal class SteamAppData
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("short_description")] public string? ShortDescription { get; set; }
        [JsonPropertyName("detailed_description")] public string? DetailedDescription { get; set; }
        [JsonPropertyName("header_image")] public string? HeaderImage { get; set; }
        [JsonPropertyName("background")] public string? Background { get; set; }
        [JsonPropertyName("background_raw")] public string? BackgroundRaw { get; set; }
        [JsonPropertyName("website")] public string? Website { get; set; }
        [JsonPropertyName("developers")] public List<string>? Developers { get; set; }
        [JsonPropertyName("publishers")] public List<string>? Publishers { get; set; }
        [JsonPropertyName("metacritic")] public SteamMetacritic? Metacritic { get; set; }
        [JsonPropertyName("genres")] public List<SteamGenre>? Genres { get; set; }
        [JsonPropertyName("categories")] public List<SteamGenre>? Categories { get; set; }
        [JsonPropertyName("screenshots")] public List<SteamScreenshot>? Screenshots { get; set; }
        [JsonPropertyName("movies")] public List<SteamMovie>? Movies { get; set; }
        [JsonPropertyName("release_date")] public SteamReleaseDate? ReleaseDate { get; set; }
        [JsonPropertyName("is_free")] public bool IsFree { get; set; }
        [JsonPropertyName("price_overview")] public SteamPrice? PriceOverview { get; set; }
        [JsonPropertyName("platforms")] public SteamPlatforms? Platforms { get; set; }
        [JsonPropertyName("tags")] public List<SteamTag>? Tags { get; set; }
    }

    internal class SteamMetacritic { [JsonPropertyName("score")] public int Score { get; set; } }
    internal class SteamGenre { [JsonPropertyName("description")] public string? Description { get; set; } }
    internal class SteamTag { [JsonPropertyName("name")] public string? Name { get; set; } }
    internal class SteamScreenshot { [JsonPropertyName("path_full")] public string? PathFull { get; set; } }
    internal class SteamReleaseDate { [JsonPropertyName("date")] public string? Date { get; set; } }
    internal class SteamPrice
    {
        [JsonPropertyName("final")] public int Final { get; set; }  // cents
        [JsonPropertyName("currency")] public string? Currency { get; set; }
    }
    internal class SteamPlatforms
    {
        [JsonPropertyName("windows")] public bool Windows { get; set; }
        [JsonPropertyName("mac")] public bool Mac { get; set; }
        [JsonPropertyName("linux")] public bool Linux { get; set; }
    }
    internal class SteamMovie
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("thumbnail")] public string? Thumb { get; set; }
        [JsonPropertyName("webm")] public SteamVideoSources? Webm { get; set; }
        [JsonPropertyName("mp4")] public SteamVideoSources? Mp4 { get; set; }
        [JsonPropertyName("highlight")] public bool Highlight { get; set; }
    }
    internal class SteamVideoSources { [JsonPropertyName("max")] public string? Max { get; set; } }

    // ══════════════════════════════════════════════════════
    //  CATEGORY SERVICE
    // ══════════════════════════════════════════════════════
    public interface IStreamCategoryService
    {
        Task<IEnumerable<StreamCategoryDto>> GetAllAsync();
        Task<StreamCategoryDto?> GetByIdAsync(int id);
        Task<StreamCategoryDto> CreateAsync(CreateCategoryRequest request);
        Task<StreamCategoryDto> SyncFromSteamAsync(int steamAppId);
        Task<StreamCategoryDto?> UpdateStatsAsync(int categoryId);
        Task<bool> DeleteAsync(int id);
    }

    public class StreamCategoryService : IStreamCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpFactory;

        public StreamCategoryService(AppDbContext context, IHttpClientFactory httpFactory)
        {
            _context = context;
            _httpFactory = httpFactory;
        }

        public async Task<IEnumerable<StreamCategoryDto>> GetAllAsync()
        {
            var cats = await _context.StreamCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.PopularityRank ?? 9999)
                .ThenByDescending(c => c.CurrentViewers)
                .ToListAsync();
            return cats.Select(MapToDto);
        }

        public async Task<StreamCategoryDto?> GetByIdAsync(int id)
        {
            var cat = await _context.StreamCategories.FindAsync(id);
            return cat == null ? null : MapToDto(cat);
        }

        public async Task<StreamCategoryDto> CreateAsync(CreateCategoryRequest request)
        {
            // Nếu có SteamAppId → sync luôn từ Steam
            if (request.SteamAppId.HasValue)
                return await SyncFromSteamAsync(request.SteamAppId.Value);

            var cat = new StreamCategory
            {
                Name = request.Name,
                Description = request.Description,
                Slug = Slugify(request.Name),
                CoverImageUrl = request.CoverImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.StreamCategories.Add(cat);
            await _context.SaveChangesAsync();
            return MapToDto(cat);
        }

        /// <summary>
        /// Lấy thông tin game từ Steam Store API và lưu vào DB
        /// Steam API: https://store.steampowered.com/api/appdetails?appids={appId}
        /// </summary>
        public async Task<StreamCategoryDto> SyncFromSteamAsync(int steamAppId)
        {
            var http = _httpFactory.CreateClient();
            var url = $"https://store.steampowered.com/api/appdetails?appids={steamAppId}&l=english";
            var json = await http.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement.GetProperty(steamAppId.ToString());
            var appDetails = JsonSerializer.Deserialize<SteamAppDetailsResponse>(
                root.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (appDetails?.Success != true || appDetails.Data == null)
                throw new Exception($"Steam App ID {steamAppId} not found.");

            var d = appDetails.Data;

            // Tìm existing hoặc tạo mới
            var cat = await _context.StreamCategories
                .FirstOrDefaultAsync(c => c.SteamAppId == steamAppId)
                ?? new StreamCategory { CreatedAt = DateTime.UtcNow };

            // ── Map dữ liệu từ Steam ──────────────────────
            cat.SteamAppId = steamAppId;
            cat.Name = d.Name ?? cat.Name;
            cat.Slug = Slugify(d.Name ?? "");
            cat.Description = StripHtml(d.ShortDescription ?? d.DetailedDescription ?? "");
            cat.CoverImageUrl = d.HeaderImage;
            cat.BackgroundImageUrl = d.BackgroundRaw ?? d.Background;
            cat.Developer = string.Join(", ", d.Developers ?? new());
            cat.Publisher = string.Join(", ", d.Publishers ?? new());
            cat.MetacriticScore = d.Metacritic?.Score;
            cat.IsFreeToPlay = d.IsFree;
            cat.SteamStoreUrl = $"https://store.steampowered.com/app/{steamAppId}";
            cat.OfficialWebsiteUrl = d.Website;
            cat.UpdatedAt = DateTime.UtcNow;
            cat.IsActive = true;

            // Genres
            cat.Genres = d.Genres != null
                ? string.Join(",", d.Genres.Select(g => g.Description))
                : cat.Genres;

            // Steam tags
            cat.SteamTags = d.Tags != null
                ? string.Join(",", d.Tags.Take(10).Select(t => t.Name))
                : cat.SteamTags;

            // Platforms
            if (d.Platforms != null)
            {
                var platforms = new List<string>();
                if (d.Platforms.Windows) platforms.Add("PC");
                if (d.Platforms.Mac) platforms.Add("Mac");
                if (d.Platforms.Linux) platforms.Add("Linux");
                cat.Platforms = string.Join(",", platforms);
            }

            // Price
            if (d.PriceOverview != null)
            {
                cat.Price = d.PriceOverview.Final / 100m;
                cat.Currency = d.PriceOverview.Currency;
            }

            // Release date
            if (d.ReleaseDate?.Date != null &&
                DateTime.TryParse(d.ReleaseDate.Date, out var releaseDate))
                cat.ReleaseDate = releaseDate;

            // Screenshots (lấy tối đa 8)
            if (d.Screenshots?.Any() == true)
                cat.ScreenshotsJson = JsonSerializer.Serialize(
                    d.Screenshots.Take(8).Select(s => s.PathFull).ToList()
                );

            // Trailer video (lấy cái highlight đầu tiên)
            var trailer = d.Movies?.FirstOrDefault(m => m.Highlight)
                       ?? d.Movies?.FirstOrDefault();
            if (trailer != null)
            {
                cat.TrailerVideoUrl = trailer.Mp4?.Max ?? trailer.Webm?.Max;
                cat.BannerImageUrl = trailer.Thumb;
            }

            if (cat.Id == 0) _context.StreamCategories.Add(cat);
            await _context.SaveChangesAsync();
            return MapToDto(cat);
        }

        /// <summary>Cập nhật số viewer và stream đang live cho category</summary>
        public async Task<StreamCategoryDto?> UpdateStatsAsync(int categoryId)
        {
            var cat = await _context.StreamCategories.FindAsync(categoryId);
            if (cat == null) return null;

            cat.ActiveStreamsCount = await _context.Streams
                .CountAsync(s => s.CategoryId == categoryId && s.Status == "live");

            cat.CurrentViewers = await _context.Streams
                .Where(s => s.CategoryId == categoryId && s.Status == "live")
                .SumAsync(s => s.ViewersCount);

            cat.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapToDto(cat);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var cat = await _context.StreamCategories.FindAsync(id);
            if (cat == null) return false;

            // KIỂM TRA RÀNG BUỘC: Nếu có Stream nào đang thuộc Category này thì không cho xóa
            var hasRelatedStreams = await _context.Streams.AnyAsync(s => s.CategoryId == id);
            if (hasRelatedStreams)
            {
                throw new Exception("Không thể xóa danh mục này vì đang có các Stream thuộc danh mục này!");
            }

            _context.StreamCategories.Remove(cat);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Helpers ──────────────────────────────────────
        private static StreamCategoryDto MapToDto(StreamCategory c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Slug = c.Slug,
            CoverImageUrl = c.CoverImageUrl,
            BannerImageUrl = c.BannerImageUrl,
            BackgroundImageUrl = c.BackgroundImageUrl,
            IconUrl = c.IconUrl,
            TrailerVideoUrl = c.TrailerVideoUrl,
            GameplayVideoUrl = c.GameplayVideoUrl,
            Screenshots = TryDeserialize(c.ScreenshotsJson),
            Developer = c.Developer,
            Publisher = c.Publisher,
            ReleaseDate = c.ReleaseDate,
            Genres = c.Genres?.Split(',').ToList() ?? new(),
            Platforms = c.Platforms?.Split(',').ToList() ?? new(),
            SteamTags = c.SteamTags?.Split(',').ToList() ?? new(),
            MetacriticScore = c.MetacriticScore,
            SteamReviewScore = c.SteamReviewScore,
            SteamReviewCount = c.SteamReviewCount,
            Price = c.Price,
            Currency = c.Currency,
            IsFreeToPlay = c.IsFreeToPlay,
            SteamStoreUrl = c.SteamStoreUrl,
            OfficialWebsiteUrl = c.OfficialWebsiteUrl,
            CurrentViewers = c.CurrentViewers,
            ActiveStreamsCount = c.ActiveStreamsCount,
            PopularityRank = c.PopularityRank,
            SteamAppId = c.SteamAppId,
        };

        private static List<string> TryDeserialize(string? json)
        {
            if (string.IsNullOrEmpty(json)) return new();
            try { return JsonSerializer.Deserialize<List<string>>(json) ?? new(); }
            catch { return new(); }
        }

        private static string Slugify(string name) =>
            System.Text.RegularExpressions.Regex
                .Replace(name.ToLower().Trim(), @"[^a-z0-9]+", "-")
                .Trim('-');

        private static string StripHtml(string html) =>
            System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", "").Trim();
    }
}