using Microsoft.EntityFrameworkCore;
using Meetingroom.Models;

namespace Meetingroom.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MeetingRoomBooking> MeetingRoomBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MeetingRoomBooking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoomName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EmployeeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasConversion<int>();
            });
        }
    }
}