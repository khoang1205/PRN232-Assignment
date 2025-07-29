using Microsoft.EntityFrameworkCore;
using Notification;
using PRN232_Assignment.AppointmentService.Grpc.Controller;
using PRN232_Assignment.AppointmentService.Grpc.Data;
using PRN232_Assignment.AppointmentService.Grpc.Services;
using User;
Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Đăng ký gRPC client cho UserService và NotificationService


builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
	o.Address = new Uri("https://localhost:7073");
});

builder.Services.AddGrpcClient<NotificationService.NotificationServiceClient>(o =>
{
	o.Address = new Uri("https://localhost:7075");
});
builder.Services.AddHttpClient<DoctorClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7238");
});
builder.Services.AddScoped<IAppointmentService, AppointmentBusinessService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AppointmentGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
