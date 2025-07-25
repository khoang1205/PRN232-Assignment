using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Service.Models;

namespace PRN232_Assignment.UserService.Service.IService
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<LoginResponseDto> Login(string email, string password);
        Task<bool> Register(RegisterDto registerDto);
    }
}