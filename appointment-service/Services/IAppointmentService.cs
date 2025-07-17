using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appointment_service.Services
{
    public interface IAppointmentService
    {
        Task<string> CreateAsync(string doctorId, string patientId, string timeSlot);
    }
}
