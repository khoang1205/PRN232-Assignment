using appointment_service.Controller;
using appointment_service.Data;
using appointment_service.Services;
using Microsoft.EntityFrameworkCore;

namespace appointmentservice
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
