var builder = WebApplication.CreateBuilder(args);


builder.Services.AddGrpc();
builder.Services.AddLogging();

var app = builder.Build();


app.MapGrpcService<notification_service.Services.NotificationGrpcService>();
app.MapGet("/", () => "This service uses gRPC. To test it, use a gRPC client.");

app.Run();
