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
        public DbSet<ChatBan> ChatBan { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<StreamCategory> StreamCategories { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlan { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<hoangngocthe_2123110488.Model.Stream> Streams { get; set; }

        // Phương thức này PHẢI nằm bên trong dấu ngoặc của class AppDbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình để tránh lỗi "multiple cascade paths"
            modelBuilder.Entity<ChatMessage>()
                .HasOne(m => m.User)
                .WithMany(u => u.ChatMessages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Đổi sang Restrict

            // Lưu ý: Nếu bạn gặp lỗi tương tự ở các bảng khác (như Follow, Donation), 
            // bạn cũng cần cấu hình .OnDelete(DeleteBehavior.Restrict) cho chúng tại đây.
        }
    } // Kết thúc class AppDbContext
} // Kết thúc namespace