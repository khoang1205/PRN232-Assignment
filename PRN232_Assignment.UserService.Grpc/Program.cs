using Microsoft.EntityFrameworkCore;
using PRN232_Assignment.UserService.Data;
using PRN232_Assignment.UserService.Services;

namespace userservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddScoped<IUserService, UserService>();

            
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddGrpc();

            var app = builder.Build();

            
            app.MapGrpcService<UserGrpcService>();

            app.MapGet("/", () => "This server hosts a gRPC service. Use a gRPC client to call endpoints.");

            app.Run();
        }
    }
}
