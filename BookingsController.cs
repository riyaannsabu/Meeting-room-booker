using Microsoft.AspNetCore.Mvc;
using Meetingroom.DTOs;
using Meetingroom.Models;
using Meetingroom.Repositories;

namespace Meetingroom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _repository;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingRepository repository, ILogger<BookingsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all meeting room bookings
        /// </summary>
        /// <returns>List of all bookings</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookingResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings()
        {
            var bookings = await _repository.GetAllAsync();
            var response = bookings.Select(BookingResponseDto.FromModel);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific booking by ID
        /// </summary>
        /// <param name="id">The booking ID</param>
        /// <returns>The booking details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDto>> GetBooking(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(BookingResponseDto.FromModel(booking));
        }

        /// <summary>
        /// Creates a new booking request
        /// </summary>
        /// <param name="request">Booking request details</param>
        /// <returns>The created booking</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] BookingRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if room is available
            var isAvailable = await _repository.IsRoomAvailableAsync(request.RoomName, request.BookingDateTime);
            if (!isAvailable)
                return BadRequest(new { message = "Room is not available for the selected time" });

            var booking = new MeetingRoomBooking
            {
                RoomName = request.RoomName,
                EmployeeName = request.EmployeeName,
                BookingDateTime = request.BookingDateTime,
                Status = BookingStatus.Pending
            };

            var created = await _repository.CreateAsync(booking);
            var response = BookingResponseDto.FromModel(created);

            return CreatedAtAction(nameof(GetBooking), new { id = created.Id }, response);
        }

        /// <summary>
        /// Confirms a booking (Admin only)
        /// </summary>
        /// <param name="request">Confirmation request with booking ID</param>
        /// <returns>The confirmed booking</returns>
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDto>> ConfirmBooking([FromBody] ConfirmBookingDto request)
        {
            var booking = await _repository.GetByIdAsync(request.BookingId);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            if (booking.Status == BookingStatus.Confirmed)
                return BadRequest(new { message = "Booking is already confirmed. Cannot book again." });

            booking.Status = BookingStatus.Confirmed;
            var updated = await _repository.UpdateAsync(booking);

            return Ok(BookingResponseDto.FromModel(updated!));
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="id">The booking ID to cancel</param>
        /// <returns>No content</returns>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            booking.Status = BookingStatus.Cancelled;
            await _repository.UpdateAsync(booking);

            return NoContent();
        }

        /// <summary>
        /// Deletes a booking
        /// </summary>
        /// <param name="id">The booking ID to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Booking not found" });

            return NoContent();
        }
    }
}