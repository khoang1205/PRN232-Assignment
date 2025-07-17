using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorNow.Domain.Common;

namespace DoctorNow.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}
