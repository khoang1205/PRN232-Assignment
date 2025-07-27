using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MongoDB.Bson;
using PRN232_Assignment.DoctorService.Repository.Entities;
using PRN232_Assignment.DoctorService.Repository.IRepository;
using PRN232_Assignment.DoctorService.Service.IService;
using PRN232_Assignment.DoctorService.Service.Models.Request;

namespace PRN232_Assignment.DoctorService.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly Cloudinary _cloudinary;

        public DoctorService(IDoctorRepository repo, Cloudinary cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Doctor?> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Doctor> CreateAsync(DoctorCreateRequest request)
        {
            string? avatarUrl = null;

            // Upload avatar nếu có
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

            // Cập nhật các field cơ bản
            existingDoctor.FullName = request.FullName.Trim();
            existingDoctor.Email = request.Email.Trim().ToLower();
            existingDoctor.Specialty = request.Specialty.Trim();
            existingDoctor.Bio = string.IsNullOrWhiteSpace(request.Bio) ? string.Empty : request.Bio;

            // Cập nhật avatar nếu có
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
    }
}
