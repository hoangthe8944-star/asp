using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hoangngocthe_2123110488.Model
{
    /// <summary>
    /// Category / Game — tích hợp metadata từ Steam / IGDB / RAWG
    /// </summary>
    public class StreamCategory
    {
        [Key]
        public int Id { get; set; }

        // ── THÔNG TIN CƠ BẢN ─────────────────────────────
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>Slug dùng cho URL: "counter-strike-2"</summary>
        [StringLength(120)]
        public string? Slug { get; set; }

        // ── MEDIA ─────────────────────────────────────────
        /// <summary>Ảnh thumbnail/cover nhỏ (hiện trong danh sách)</summary>
        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        /// <summary>Ảnh banner lớn (hiện trên trang category)</summary>
        [StringLength(500)]
        public string? BannerImageUrl { get; set; }

        /// <summary>Ảnh background (dùng làm nền trang stream)</summary>
        [StringLength(500)]
        public string? BackgroundImageUrl { get; set; }

        /// <summary>Icon nhỏ (16x16 hoặc 32x32)</summary>
        [StringLength(500)]
        public string? IconUrl { get; set; }

        /// <summary>URL trailer / gameplay video (YouTube hoặc Steam CDN)</summary>
        [StringLength(500)]
        public string? TrailerVideoUrl { get; set; }

        /// <summary>URL video gameplay mẫu</summary>
        [StringLength(500)]
        public string? GameplayVideoUrl { get; set; }

        /// <summary>Danh sách ảnh screenshot (JSON array of URLs)</summary>
        public string? ScreenshotsJson { get; set; }

        // ── GAME METADATA ─────────────────────────────────
        /// <summary>App ID trên Steam (ví dụ: 730 = CS2)</summary>
        public int? SteamAppId { get; set; }

        /// <summary>ID trên IGDB (igdb.com)</summary>
        public int? IgdbId { get; set; }

        /// <summary>ID trên RAWG (rawg.io)</summary>
        [StringLength(100)]
        public string? RawgSlug { get; set; }

        /// <summary>ID trên GEFORCE NOW catalog</summary>
        [StringLength(100)]
        public string? GeforceNowId { get; set; }

        /// <summary>Nhà phát triển game</summary>
        [StringLength(200)]
        public string? Developer { get; set; }

        /// <summary>Nhà phát hành game</summary>
        [StringLength(200)]
        public string? Publisher { get; set; }

        /// <summary>Ngày phát hành</summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Thể loại game: "FPS,Battle Royale,Tactical"</summary>
        [StringLength(300)]
        public string? Genres { get; set; }

        /// <summary>Nền tảng: "PC,PS5,Xbox,Mobile"</summary>
        [StringLength(200)]
        public string? Platforms { get; set; }

        /// <summary>Tags từ Steam: "Multiplayer,Competitive,Free to Play"</summary>
        [StringLength(500)]
        public string? SteamTags { get; set; }

        /// <summary>Điểm đánh giá trên Metacritic (0-100)</summary>
        public int? MetacriticScore { get; set; }

        /// <summary>Điểm đánh giá trên Steam (0-100)</summary>
        public int? SteamReviewScore { get; set; }

        /// <summary>Số lượng đánh giá trên Steam</summary>
        public int? SteamReviewCount { get; set; }

        /// <summary>Giá gốc (VND hoặc USD)</summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        /// <summary>Đơn vị tiền tệ: "USD" hoặc "VND"</summary>
        [StringLength(10)]
        public string? Currency { get; set; } = "USD";

        /// <summary>Free to play hay không</summary>
        public bool IsFreeToPlay { get; set; } = false;

        /// <summary>Link mua game trên Steam</summary>
        [StringLength(300)]
        public string? SteamStoreUrl { get; set; }

        /// <summary>Link trang chủ game</summary>
        [StringLength(300)]
        public string? OfficialWebsiteUrl { get; set; }

        // ── STREAMING STATS ───────────────────────────────
        /// <summary>Tổng số viewer hiện tại trên platform</summary>
        public int CurrentViewers { get; set; } = 0;

        /// <summary>Tổng số stream đang live</summary>
        public int ActiveStreamsCount { get; set; } = 0;

        /// <summary>Rank phổ biến trên platform (1 = phổ biến nhất)</summary>
        public int? PopularityRank { get; set; }

        // ── SYSTEM ────────────────────────────────────────
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ── NAVIGATION ────────────────────────────────────
        public ICollection<Stream> Streams { get; set; } = new List<Stream>();
    }
}