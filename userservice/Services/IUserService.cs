using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user_service.Entities;

namespace userservice.Services
{
    public interface IUserService
    {
        Task<user_service.Entities.User> GetByIdAsync(Guid id);
        Task<List<user_service.Entities.User>> GetAllAsync();
        Task<Guid> CreateAsync(string name, string role);
    }
}
