using user_service.Services; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace userservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc(); // Requires Microsoft.AspNetCore.Grpc NuGet package

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserGrpcService>(); // Use your actual gRPC service
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
