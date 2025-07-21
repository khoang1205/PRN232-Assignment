using PRN232_Assignment.DoctorService.Repository.Entities;
using PRN232_Assignment.DoctorService.Service.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.DoctorService.Service.IService
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(string id);
        Task<Doctor> CreateAsync(DoctorCreateRequest doctor);
        Task<bool> UpdateAsync(string id, Doctor doctor);
        Task<bool> DeleteAsync(string id);
        Task<List<Doctor>> SearchAsync(string? name, string? specialty);
    }
}
