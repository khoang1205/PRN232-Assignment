namespace PRN232_Assignment.UserService.Service.Models
{
    public class LoginResponseDto
    {
        public Guid id { get; set; }
        public string Token { get; set; }
        public int RoleId { get; set; }
    }
}
