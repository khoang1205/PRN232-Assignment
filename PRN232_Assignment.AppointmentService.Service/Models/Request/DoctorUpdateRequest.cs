using Microsoft.AspNetCore.Http;

namespace PRN232_Assignment.DoctorService.Service.Models.Request
{
    public class DoctorUpdateRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Specialty { get; set; } = null!;
        public string? Bio { get; set; }
        public int? Experience { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
