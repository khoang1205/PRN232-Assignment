using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PRN232_Assignment.AppointmentService.Grpc.DTO;

namespace PRN232_Assignment.AppointmentService.Grpc.Services
{
    public class UserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5012");
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/User/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(json);
        }

        public async Task<string> GetUserNameByIdAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            return user?.Name ?? "Bệnh nhân";
        }
    }
}
