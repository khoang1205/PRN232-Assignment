namespace PRN232_Assignment.UserService.Services
{
    public interface IUserService
    {
        Task<Entities.User> GetByIdAsync(Guid id);
        Task<List<Entities.User>> GetAllAsync();
        Task<Guid> CreateAsync(string name, string role);
    }
}
