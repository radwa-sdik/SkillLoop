using Microsoft.EntityFrameworkCore;
using SkillLoop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillLoop.Infrasturcture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<User> Users => Set<User>();
        public DbSet<ExternalLogin> ExternalLogins => Set<ExternalLogin>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<CourseTag> CourseTags => Set<CourseTag>();
        public DbSet<LearningPoint> LearningPoints => Set<LearningPoint>();
        public DbSet<SavedCourse> SavedCourses => Set<SavedCourse>();

        public DbSet<Slot> Slots => Set<Slot>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Booking> Bookings => Set<Booking>();

        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
        public DbSet<CreditPackage> CreditPackages => Set<CreditPackage>();
        public DbSet<PaymentOrder> PaymentOrders => Set<PaymentOrder>();
        public DbSet<PromoCode> PromoCodes => Set<PromoCode>();
        public DbSet<PromoRedemption> PromoRedemptions => Set<PromoRedemption>();

        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();

        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Notification> Notifications => Set<Notification>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
