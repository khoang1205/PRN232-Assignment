using System.ComponentModel.DataAnnotations;

namespace PRN232_Assignment.UserService.Service.Models
{
    public class RegisterDto
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; } // 1: Docter, 2: Patient
    }
}
