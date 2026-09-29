using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SessionType)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.MeetingLink)
                .HasMaxLength(1000);

            builder.HasIndex(x => new { x.SlotId, x.Status });

            builder.HasOne(x => x.Enrollment)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Slot)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.SlotId)
                .OnDelete(DeleteBehavior.Restrict);
        }
}
}
