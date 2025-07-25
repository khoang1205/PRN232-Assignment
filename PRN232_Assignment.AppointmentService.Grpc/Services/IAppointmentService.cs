using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.AppointmentService.Services
{
    public interface IAppointmentService
    {
        Task<string> CreateAsync(string doctorId, string patientId, string timeSlot);
    }
}
