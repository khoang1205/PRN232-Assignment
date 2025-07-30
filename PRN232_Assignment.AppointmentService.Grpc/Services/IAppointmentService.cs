using PRN232_Assignment.AppointmentService.Grpc.Entities;
using PRN232_Assignment.AppointmentService.Grpc.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.AppointmentService.Grpc.Services
{
    public interface IAppointmentService
    {
		Task<string> CreateScheduleAsync(string doctorId, string date);
		Task<List<TimeSlot>> GetAvailableSlotsAsync(string doctorId, string date);
		Task<string> BookSlotAsync(string slotId, string patientId);
		Task<List<BookedTimeSlotDetail>> GetBookedTimeSlotsByPatientIdAsync(string patientId);
        Task<List<TimeSlot>> GetBookedSlotsByDoctorAsync(string doctorId, string dateStr);
    

        Task<string> GetDoctorIdFromSlotAsync(string slotId);
    }
}
