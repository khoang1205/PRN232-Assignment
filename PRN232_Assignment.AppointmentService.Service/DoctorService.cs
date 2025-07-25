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

        public DoctorService(IDoctorRepository repo)
        {
            _repo = repo;
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
            var doctor = new Doctor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                Specialty = request.Specialty.Trim(),
                Bio = string.IsNullOrWhiteSpace(request.Bio) ? string.Empty : request.Bio,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(doctor);
            return doctor;
        }

        public async Task<bool> UpdateAsync(string id, Doctor doctor)
        {
            return await _repo.UpdateAsync(id, doctor);
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
