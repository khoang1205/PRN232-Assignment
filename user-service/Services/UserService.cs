using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_service.Data;
using user_service.Entities;

namespace user_service.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<user_service.Entities.User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<user_service.Entities.User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Guid> CreateAsync(string name, string role)
        {
            var user = new user_service.Entities.User { Name = name, Role = role };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
