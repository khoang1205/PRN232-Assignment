using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Service.Models;
using EntityUser = PRN232_Assignment.UserService.Repository.Entities.User;
namespace PRN232_Assignment.UserService.Service.IService
{
    public interface IUserService
    {
        Task<List<EntityUser>> GetAllAsync();
        Task<EntityUser> GetByIdAsync(Guid id);
        Task<LoginResponseDto> Login(string email, string password);
        Task<bool> Register(RegisterDto registerDto);
    }
}