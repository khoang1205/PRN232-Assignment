using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.UserService.Services
{
    public interface IUserService
    {
        Task<Entities.User> GetByIdAsync(Guid id);
        Task<List<Entities.User>> GetAllAsync();
        Task<Guid> CreateAsync(string name, string role);
    }
}
