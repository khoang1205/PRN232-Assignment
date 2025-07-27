using CloudinaryDotNet;
using MongoDB.Driver;
using PRN232_Assignment.DoctorService.Repository;
using PRN232_Assignment.DoctorService.Repository.Entities;
using PRN232_Assignment.DoctorService.Repository.IRepository;
using PRN232_Assignment.DoctorService.Service;
using PRN232_Assignment.DoctorService.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================================================================================== //

// DI
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

// ========================================================================================================//

// MongoDB configuration
string mongoConnectionString = builder.Configuration.GetConnectionString("Mongo");
string databaseName = "DoctorNow";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

// ======================================================================================================== //
// Cloudinary configuration
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var cloudName = config["Cloudinary:CloudName"];
    var apiKey = config["Cloudinary:ApiKey"];
    var apiSecret = config["Cloudinary:ApiSecret"];

    var account = new Account(cloudName, apiKey, apiSecret);
    return new Cloudinary(account);
});
// ======================================================================================================== //

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
