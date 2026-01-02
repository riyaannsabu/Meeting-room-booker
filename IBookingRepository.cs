using Meetingroom.Models;

namespace Meetingroom.Repositories
{
    public interface IBookingRepository
    {
        Task<MeetingRoomBooking?> GetByIdAsync(int id);
        Task<IEnumerable<MeetingRoomBooking>> GetAllAsync();
        Task<MeetingRoomBooking> CreateAsync(MeetingRoomBooking booking);
        Task<MeetingRoomBooking?> UpdateAsync(MeetingRoomBooking booking);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsRoomAvailableAsync(string roomName, DateTime bookingDateTime);
    }
}