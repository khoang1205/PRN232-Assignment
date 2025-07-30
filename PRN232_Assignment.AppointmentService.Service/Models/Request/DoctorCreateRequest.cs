using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PRN232_Assignment.DoctorService.Service.Models.Request
{
    public class DoctorCreateRequest
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; } = null!;

        [StringLength(1000)]
        public string? Bio { get; set; }

        [Range(0, 50)]
        public int? Experience { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
