using Microsoft.AspNetCore.Mvc;
using api_gateway.Models;
using appointment_service.Services;
using appointment_service.Entities;
using Appointment;

namespace api_gateway.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentResponse>> Create([FromBody] CreateAppointmentRequest request)
        {
            // Convert string to Guid and DateTime
            var appointmentId = await _appointmentService.CreateAsync(
             request.DoctorId,
             request.PatientId,
             request.TimeSlot
            );

            return Ok(new AppointmentResponse
            {
                AppointmentId = appointmentId.ToString(),
                Status = "Created"
            });
        }
    }
}
