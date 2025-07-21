using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Appointment;
using PRN232_Assignment.AppointmentService.Services;
namespace PRN232.AppointmentService.Controller
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