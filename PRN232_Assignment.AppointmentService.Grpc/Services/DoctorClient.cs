using Newtonsoft.Json;
using PRN232_Assignment.AppointmentService.Grpc.DTO;


namespace PRN232_Assignment.AppointmentService.Grpc.Services
{
    public class DoctorClient
    {
        private readonly HttpClient _httpClient;

        public DoctorClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7238");
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            var response = await _httpClient.GetAsync("/doctors");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorDto>>(content);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(string id)
        {
            var doctors = await GetDoctorsAsync();
            return doctors.FirstOrDefault(d => d.Id == id);
        }


    }
}
