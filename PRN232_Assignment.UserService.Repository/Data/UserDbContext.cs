using System;
using Microsoft.EntityFrameworkCore;
using PRN232_Assignment.UserService.Repository.Entities;

namespace PRN232_Assignment.UserService.Repository.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Doctor" },
                new Role { Id = 2, Name = "Patient" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Doctor User",
                    Email = "doctor@example.com",
                    Password = "1",
                    RoleId = 1
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Patient User",
                    Email = "patient@example.com",
                    Password = "1",
                    RoleId = 2
                }
            );
        }
    }
}
