using System;

namespace PRN232_Assignment.AppointmentService.Grpc.DTO
{
    public class BookedTimeSlotDetail
    {
        public Guid SlotId { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public Guid? PatientId { get; set; }
    }
} 