using System;

namespace PRN232_Assignment.AppointmentService.Grpc.DTO
{
    public class AppointmentWithDoctorInfo
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Status { get; set; }
    }
} 