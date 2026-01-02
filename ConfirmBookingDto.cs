using System.ComponentModel.DataAnnotations;

namespace Meetingroom.DTOs
{
    public class ConfirmBookingDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}