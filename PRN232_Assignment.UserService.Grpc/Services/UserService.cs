using Microsoft.EntityFrameworkCore;
using PRN232_Assignment.UserService.Data;

namespace PRN232_Assignment.UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<Entities.User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Guid> CreateAsync(string name, string role)
        {
            var user = new Entities.User { Name = name, Role = role };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
