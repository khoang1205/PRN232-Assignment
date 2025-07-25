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
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; } = null!;

        [StringLength(1000)]
        public string? Bio { get; set; }

    }
}
