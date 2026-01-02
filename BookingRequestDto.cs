using System.ComponentModel.DataAnnotations;

namespace Meetingroom.DTOs
{
    public class BookingRequestDto
    {
        [Required(ErrorMessage = "Room name is required")]
        public string RoomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee name is required")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Booking date and time is required")]
        public DateTime BookingDateTime { get; set; }
    }
}