using Microsoft.AspNetCore.Mvc.RazorPages;
using Meetingroom.DTOs;
using System.Text.Json;

namespace Meetingroom.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<BookingResponseDto> Bookings { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            try
            {
                var response = await client.GetAsync($"{baseUrl}/api/bookings");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Bookings = JsonSerializer.Deserialize<List<BookingResponseDto>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
            }
            catch
            {
                Bookings = new();
            }
        }
    }
}