using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Meetingroom.DTOs;
using System.Text;
using System.Text.Json;

namespace Meetingroom.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public BookingRequestDto BookingRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public bool SuccessMessage { get; set; }

        public void OnGet()
        {
            // Set default datetime to current time + 1 hour
            BookingRequest.BookingDateTime = DateTime.Now.AddHours(1);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            try
            {
                var json = JsonSerializer.Serialize(BookingRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{baseUrl}/api/bookings", content);

                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = true;
                    return Page();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = "Failed to create booking. The room might not be available.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}