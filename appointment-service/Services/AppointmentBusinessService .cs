using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appointment_service.Services
{
    public class AppointmentBusinessService : IAppointmentService
    {
        public async Task<string> CreateAsync(string doctorId, string patientId, string timeSlot)
        {
            // TODO: Lưu vào DB (EF Core)
            // Tạm thời return fake ID
            await Task.Delay(10); // giả lập async
            return Guid.NewGuid().ToString();
        }
    }
}
