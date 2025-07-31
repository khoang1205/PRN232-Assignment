using PRN232_Assignment.NotificationServices.Grpc.Hubs;
using PRN232_Assignment.NotificationServices.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.MapHub<NotificationHub>("/notificationhub")
   .RequireCors(policy => policy
      .WithOrigins("http://localhost:8080")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials());

app.MapGrpcService<NotificationGrpcService>();
// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseCors();
app.Run();
