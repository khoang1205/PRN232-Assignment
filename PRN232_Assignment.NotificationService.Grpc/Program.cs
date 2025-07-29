using PRN232_Assignment.NotificationServices.Grpc.Hubs;
using PRN232_Assignment.NotificationServices.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSignalR();


var app = builder.Build();
app.MapHub<NotificationHub>("/notificationhub");
app.MapGrpcService<NotificationGrpcService>();
// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
