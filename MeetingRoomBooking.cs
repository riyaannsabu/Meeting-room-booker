namespace Meetingroom.Models
{
    public class MeetingRoomBooking
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime BookingDateTime { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}