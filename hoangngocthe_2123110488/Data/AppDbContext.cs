using System.Composition;
using System.Data;
using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace hoangngocthe_2123110488.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── USER & AUTH ───────────────────────────────────
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // ── STREAM ────────────────────────────────────────
        public DbSet<hoangngocthe_2123110488.Model.Stream> Streams { get; set; }
        public DbSet<StreamCategory> StreamCategories { get; set; }
        public DbSet<StreamTag> StreamTags { get; set; }
        public DbSet<StreamTagMapping> StreamTagMappings { get; set; }

        // ── CHAT ──────────────────────────────────────────
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatBan> ChatBans { get; set; }

        // ── SOCIAL ────────────────────────────────────────
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // ── MONETIZATION ──────────────────────────────────
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // ── ANALYTICS ─────────────────────────────────────
        public DbSet<StreamView> StreamViews { get; set; }
        public DbSet<StreamStat> StreamStats { get; set; }

        // ── ADMIN ─────────────────────────────────────────
        public DbSet<Report> Reports { get; set; }
        public DbSet<Ban> Bans { get; set; }

        // ── MEDIA ─────────────────────────────────────────
        public DbSet<Video> Videos { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }

        // ── SYSTEM ────────────────────────────────────────
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Composite PK ──────────────────────────────
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<StreamTagMapping>()
                .HasKey(stm => new { stm.StreamId, stm.TagId });

            //----------------------------------------------------
            modelBuilder.Entity<Follow>(e =>
            {
                // Cấu hình quan hệ cho người đi theo dõi
                e.HasOne(f => f.Follower)
                 .WithMany()
                 .HasForeignKey(f => f.FollowerId)
                 .OnDelete(DeleteBehavior.Restrict); // Dùng Restrict để tránh lỗi Multiple Cascade Path

                // Cấu hình quan hệ cho người được theo dõi
                e.HasOne(f => f.Following)
                 .WithMany()
                 .HasForeignKey(f => f.FollowingId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
            // ── User ──────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.Username).IsUnique();
                e.Property(u => u.Role).HasDefaultValue("viewer");
                e.Property(u => u.Status).HasDefaultValue("active");
                e.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── UserProfile → User ────────────────────────
            modelBuilder.Entity<UserProfile>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Stream → User (Streamer) ──────────────────
            modelBuilder.Entity<hoangngocthe_2123110488.Model.Stream>(e =>
            {
                e.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(s => s.StreamerId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(s => s.Status).HasDefaultValue("offline");
                e.Property(s => s.ViewersCount).HasDefaultValue(0);
            });

            // ── Follow (self-referencing) ─────────────────
            modelBuilder.Entity<Follow>(e =>
            {
                e.HasIndex(f => new { f.FollowerId, f.FollowingId }).IsUnique();
                e.Property(f => f.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── ChatMessage ───────────────────────────────
            modelBuilder.Entity<ChatMessage>(e =>
            {
                e.Property(c => c.Type).HasDefaultValue("text");
                e.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── Donation ──────────────────────────────────
            modelBuilder.Entity<Donation>(e =>
            {
                e.Property(d => d.Amount).HasColumnType("decimal(18,2)");
                e.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── SubscriptionPlan ──────────────────────────
            modelBuilder.Entity<SubscriptionPlan>(e =>
            {
                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            // ── Transaction ───────────────────────────────
            modelBuilder.Entity<Transaction>(e =>
            {
                e.Property(t => t.Amount).HasColumnType("decimal(18,2)");
                e.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── Notification ──────────────────────────────
            modelBuilder.Entity<Notification>(e =>
            {
                e.Property(n => n.IsRead).HasDefaultValue(false);
                e.Property(n => n.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── Report ────────────────────────────────────
            modelBuilder.Entity<Report>(e =>
            {
                e.Property(r => r.Status).HasDefaultValue("pending");
                e.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── Log ───────────────────────────────────────
            modelBuilder.Entity<Log>(e =>
            {
                e.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

        }
    }
}