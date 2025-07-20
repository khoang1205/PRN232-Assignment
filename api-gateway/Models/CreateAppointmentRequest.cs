namespace api_gateway.Models
{
    public class CreateAppointmentRequest
    {
        public string DoctorId { get; set; } = default!;
        public string PatientId { get; set; } = default!;
        public string TimeSlot { get; set; } = default!;
    }
}
