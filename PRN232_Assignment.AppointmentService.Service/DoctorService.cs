using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using PRN232_Assignment.DoctorService.Repository.Entities;
using PRN232_Assignment.DoctorService.Repository.IRepository;
using PRN232_Assignment.DoctorService.Service.IService;
using PRN232_Assignment.DoctorService.Service.Models.Request;
using PRN232_Assignment.DoctorService.Service.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_Assignment.DoctorService.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public DoctorService(IDoctorRepository repo, Cloudinary cloudinary, IConfiguration configuration)
        {
            _repo = repo;
            _cloudinary = cloudinary;
            _configuration = configuration;
        }

        public async Task<PagedResult<Doctor>> GetPaginatedAsync(int pageIndex, int pageSize)
        {
            var total = await _repo.CountAsync();
            var items = await _repo.GetPaginatedAsync(pageIndex, pageSize);

            return new PagedResult<Doctor>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = total,
                Items = items
            };
        }
        
        public async Task<Doctor?> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Doctor> CreateAsync(DoctorCreateRequest request)
        {
            string? avatarUrl = null;

            if (request.Avatar != null && request.Avatar.Length > 0)
            {
                using var stream = request.Avatar.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(request.Avatar.FileName, stream),
                    Folder = "doctors",
                    PublicId = $"doctor_{Guid.NewGuid()}",
                    Transformation = new Transformation()
                        .Width(300).Height(300)
                        .Crop("fill").Gravity("face")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    avatarUrl = uploadResult.SecureUrl.ToString();
                }
            }

            var doctor = new Doctor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                Password = request.Password,
                Specialty = request.Specialty.Trim(),
                Bio = string.IsNullOrWhiteSpace(request.Bio) ? string.Empty : request.Bio,
                IsActive = true,
                Avatar = avatarUrl
            };

            await _repo.CreateAsync(doctor);
            return doctor;
        }

        public async Task<bool> UpdateAsync(string id, DoctorUpdateRequest request)
        {
            var existingDoctor = await _repo.GetByIdAsync(id);
            if (existingDoctor == null)
                return false;

            if (!string.IsNullOrWhiteSpace(request.FullName))
                existingDoctor.FullName = request.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(request.Email))
                existingDoctor.Email = request.Email.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(request.Specialty))
                existingDoctor.Specialty = request.Specialty.Trim();

            if (!string.IsNullOrWhiteSpace(request.Bio))
                existingDoctor.Bio = request.Bio.Trim();

            if (request.Experience.HasValue)
                existingDoctor.Bio += $"\nExperience: {request.Experience} years";

            if (!string.IsNullOrWhiteSpace(request.Password))
                existingDoctor.Password = request.Password;

            if (request.Avatar != null && request.Avatar.Length > 0)
            {
                using var stream = request.Avatar.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(request.Avatar.FileName, stream),
                    Folder = "doctors",
                    PublicId = $"doctor_{Guid.NewGuid()}",
                    Transformation = new Transformation()
                        .Width(300).Height(300)
                        .Crop("fill").Gravity("face")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    existingDoctor.Avatar = uploadResult.SecureUrl.ToString();
                }
            }

            return await _repo.UpdateAsync(id, existingDoctor);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<List<Doctor>> SearchAsync(string? name, string? specialty)
        {
            return await _repo.SearchAsync(name, specialty);
        }

        public async Task<LoginResponse?> LoginAsync(DoctorLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var normalizedEmail = request.Email.Trim().ToLower();
            var doctor = await _repo.GetByEmailAsync(normalizedEmail);

            if (doctor == null || doctor.Password != request.Password)
                return null;

            var token = GenerateJwtToken(doctor);

            return new LoginResponse
            {
                Token = token,
                RoleId = 1
            };
        }

        private string GenerateJwtToken(Doctor doctor)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expireDays = int.Parse(_configuration["JwtSettings:ExpireDays"]!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, doctor.Id),
                new Claim(ClaimTypes.Name, doctor.FullName),
                new Claim(ClaimTypes.Email, doctor.Email),
                new Claim(ClaimTypes.Role, "Doctor")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expireDays),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
