var builder = WebApplication.CreateBuilder(args);
Console.OutputEncoding = System.Text.Encoding.UTF8;

// ✅ Đăng ký các controller
builder.Services.AddControllers();

// (Tùy chọn) Swagger nếu bạn muốn test API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ✅ Khởi tạo gRPC client để gọi qua AppointmentService
builder.Services.AddGrpcClient<Appointment.AppointmentService.AppointmentServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7074"); // địa chỉ chạy service gốc
});

// (Tùy chọn) Reverse proxy nếu bạn dùng
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.UseCors("AllowFrontend");

// Middleware
app.UseHttpsRedirection();
app.UseRouting();

// (Tùy chọn) Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Map controller routes như GatewaySlotsController
app.MapControllers();

// (Tùy chọn) nếu bạn dùng reverse proxy
app.MapReverseProxy();

app.Run();
