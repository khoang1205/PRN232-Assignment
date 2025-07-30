using Microsoft.AspNetCore.Http;

namespace PRN232_Assignment.DoctorService.Service.Models.Request
{
    public class DoctorUpdateRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Specialty { get; set; }
        public string? Bio { get; set; }
        public int? Experience { get; set; }
        public IFormFile? Avatar { get; set; }

    }
}
