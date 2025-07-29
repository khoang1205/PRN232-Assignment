using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRN232_Assignment.UserService.Api.Services;
using PRN232_Assignment.UserService.Repository.Data;
using PRN232_Assignment.UserService.Repository.Repository;
using PRN232_Assignment.UserService.Service.IService;
using PRN232_Assignment.UserService.Service.Mappings;
using System.Text;
using System.Text.Json.Serialization;

namespace PRN232_Assignment.UserService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddGrpc();
            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                });

            // Add Entity Framework Core with SQL Server
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dependency Injection
          

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserService, PRN232_Assignment.UserService.Service.Service.UserService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Configure gRPC client for AppointmentService
            builder.Services.AddGrpcClient<Appointment.AppointmentService.AppointmentServiceClient>(options =>
            {
                options.Address = new Uri("https://localhost:7074"); // gRPC service address
            });

            // Setup JWT
            builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });

            // Add SWAGGER JWT
            builder.Services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token in this field",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };
              
                c.AddSecurityRequirement(securityRequirement);
            });
            builder.WebHost.ConfigureKestrel(options =>
            {
                // HTTP
                options.ListenLocalhost(5012);

                // HTTPS for gRPC (bắt buộc gRPC phải dùng HTTP/2)
                options.ListenLocalhost(7073, listenOptions =>
                {
                    listenOptions.UseHttps(); // Ensure you have dev certificate trusted
                });
            });

            builder.Services.AddAuthorization();

            builder.Services.AddCors();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient<DoctorClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7238"); // URL của DoctorService
            });

            //===================================
            var app = builder.Build();

            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            //Use Authentication
            app.UseAuthentication();
            app.UseAuthorization();
           

            //app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // REST API
                endpoints.MapGrpcService<UserGrpcService>(); // gRPC Service
            });
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                dbContext.Database.Migrate();
            }


            app.Run();
        }
    }
}
