using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Meetingroom.DTOs;
using System.Text.Json;

namespace Meetingroom.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public BookingResponseDto? Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            try
            {
                var response = await client.GetAsync($"{baseUrl}/api/bookings/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Booking = JsonSerializer.Deserialize<BookingResponseDto>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch
            {
                Booking = null;
            }

            return Page();
        }
    }
}