using Microsoft.EntityFrameworkCore;
using PRN232.AppointmentService.Controller;
using PRN232_Assignment.AppointmentService.Data;
using PRN232_Assignment.AppointmentService.Services;

namespace PRN232_Assignment.AppointmentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddScoped<IAppointmentService, AppointmentBusinessService>();

            
            builder.Services.AddGrpc();

            var app = builder.Build();

            
            app.MapGrpcService<AppointmentGrpcService>();

            app.MapGet("/", () => "This server hosts gRPC services. Use a gRPC client to interact.");

            app.Run();
        }
    }
}
