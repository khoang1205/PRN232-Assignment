namespace PRN232_Assignment.AppointmentService.Grpc.DTO
{
    public class DoctorDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public bool IsActive { get; set; }
    }

}
