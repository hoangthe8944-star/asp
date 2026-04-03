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

            // ── UserRole: composite PK ────────────────────────
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── StreamTagMapping: composite PK ────────────────
            modelBuilder.Entity<StreamTagMapping>()
                .HasKey(s => new { s.StreamId, s.TagId });

            // ── ChatMessage ───────────────────────────────────
            modelBuilder.Entity<ChatMessage>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(c => c.Stream)
                .WithMany()
                .HasForeignKey(c => c.StreamId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── ChatBan ───────────────────────────────────────
            modelBuilder.Entity<ChatBan>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChatBan>()
                .HasOne<hoangngocthe_2123110488.Model.Stream>()
                .WithMany()
                .HasForeignKey(c => c.StreamId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── Follow: 2 FK đều trỏ về User ─────────────────
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany()
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany()
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── Donation ──────────────────────────────────────
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Donation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(d => d.StreamerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── Subscription: 2 FK đều trỏ về User ───────────
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Streamer)
                .WithMany()
                .HasForeignKey(s => s.StreamerId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── Subscription: bỏ PlanId thừa ─────────────────
            // Model có cả PlanId lẫn SubscriptionPlanId → ignore PlanId
            modelBuilder.Entity<Subscription>()
                .Ignore(s => s.PlanId);

            // ── Notification ──────────────────────────────────
            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // ── decimal columns ───────────────────────────────
            modelBuilder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}