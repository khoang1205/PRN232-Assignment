using PRN232_Assignment.DoctorService.Repository.Entities;

namespace PRN232_Assignment.DoctorService.Repository.IRepository
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetPaginatedAsync(int pageIndex, int pageSize);
        Task<long> CountAsync();
        Task<Doctor?> GetByIdAsync(string id);
        Task CreateAsync(Doctor doctor);
        Task<bool> UpdateAsync(string id, Doctor doctor);
        Task<bool> DeleteAsync(string id);
        Task<List<Doctor>> SearchAsync(string? name, string? specialty);
        Task<Doctor?> GetByEmailAsync(string email);

    }
}
