using Microsoft.EntityFrameworkCore;
using Meetingroom.Data;
using Meetingroom.Models;

namespace Meetingroom.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MeetingRoomBooking?> GetByIdAsync(int id)
        {
            return await _context.MeetingRoomBookings.FindAsync(id);
        }

        public async Task<IEnumerable<MeetingRoomBooking>> GetAllAsync()
        {
            return await _context.MeetingRoomBookings
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<MeetingRoomBooking> CreateAsync(MeetingRoomBooking booking)
        {
            _context.MeetingRoomBookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<MeetingRoomBooking?> UpdateAsync(MeetingRoomBooking booking)
        {
            var existing = await _context.MeetingRoomBookings.FindAsync(booking.Id);
            if (existing == null)
                return null;

            existing.Status = booking.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.MeetingRoomBookings.FindAsync(id);
            if (booking == null)
                return false;

            _context.MeetingRoomBookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsRoomAvailableAsync(string roomName, DateTime bookingDateTime)
        {
            return !await _context.MeetingRoomBookings
                .AnyAsync(b => b.RoomName == roomName 
                    && b.BookingDateTime == bookingDateTime 
                    && b.Status == BookingStatus.Confirmed);
        }
    }
}