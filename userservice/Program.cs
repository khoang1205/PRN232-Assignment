using Microsoft.EntityFrameworkCore;
using PRN232.UserService.Services;

namespace PRN232.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddScoped<IUserService, Services.UserService>();

            
            builder.Services.AddDbContext<PRN232.UserService.Data.UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddGrpc();

            var app = builder.Build();

            
            app.MapGrpcService<UserGrpcService>();

            app.MapGet("/", () => "This server hosts a gRPC service. Use a gRPC client to call endpoints.");

            app.Run();
        }
    }
}
