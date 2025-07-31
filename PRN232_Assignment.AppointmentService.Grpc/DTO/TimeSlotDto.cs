namespace PRN232_Assignment.AppointmentService.Grpc.DTO
{
    public class TimeSlotDto
    {
        public Guid SlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
    }

}
