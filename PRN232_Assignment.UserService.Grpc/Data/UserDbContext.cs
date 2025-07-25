using Microsoft.EntityFrameworkCore;

namespace PRN232_Assignment.UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<Entities.User> Users { get; set; }
    }

}
