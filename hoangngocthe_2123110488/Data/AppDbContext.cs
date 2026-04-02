using hoangngocthe_2123110488.Model;
using Microsoft.EntityFrameworkCore;

namespace hoangngocthe_2123110488.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<StreamView> StreamViews { get; set; }
        public DbSet<StreamStat> StreamStats { get; set; }
        public DbSet<ChatBan> ChatBans { get; set; } // Đổi tên cho đồng nhất nếu cần
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; } // CHỈ GIỮ LẠI 1 DÒNG NÀY
        public DbSet<StreamCategory> StreamCategories { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<hoangngocthe_2123110488.Model.Stream> Streams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StreamTagMapping>()
                .HasKey(st => new { st.StreamId, st.TagId });

            modelBuilder.Entity<StreamTagMapping>()
                .HasOne(st => st.Stream)
                .WithMany(s => s.StreamTagMappings)
                .HasForeignKey(st => st.StreamId);

            modelBuilder.Entity<StreamTagMapping>()
                .HasOne(st => st.Tag)
                .WithMany(t => t.StreamTagMappings)
                .HasForeignKey(st => st.TagId);
        }
    }
}