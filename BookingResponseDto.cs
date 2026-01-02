using Meetingroom.Models;

namespace Meetingroom.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime BookingDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool CanBook { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static BookingResponseDto FromModel(MeetingRoomBooking booking)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                RoomName = booking.RoomName,
                EmployeeName = booking.EmployeeName,
                BookingDateTime = booking.BookingDateTime,
                Status = booking.Status.ToString(),
                CanBook = booking.Status != BookingStatus.Confirmed,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }
    }
}