namespace PRN232_Assignment.AppointmentService.Api.Controllers
{
    public class AppointmentGrpcService : Appointment.AppointmentService.AppointmentServiceBase

    {
        private readonly IAppointmentService _service;

        public AppointmentGrpcService(IAppointmentService service)
        {
            _service = service;
        }

        public override async Task<AppointmentResponse> CreateAppointment(CreateAppointmentRequest request, ServerCallContext context)
        {
            var appointmentId = await _service.CreateAsync(request.DoctorId, request.PatientId, request.TimeSlot);
            return new AppointmentResponse { AppointmentId = appointmentId, Status = "Created" };
        }
    }
}
