using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using Appointment;

namespace PRN232_Assignment.AppointmentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService.AppointmentServiceClient _appointmentClient;

        public AppointmentController(AppointmentService.AppointmentServiceClient appointmentClient)
        {
            _appointmentClient = appointmentClient;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            try
            {
                var request = new GetAppointmentsByPatientIdRequest
                {
                    PatientId = patientId
                };

                var response = await _appointmentClient.GetAppointmentsByPatientIdAsync(request);

                return Ok(new
                {
                    Success = true,
                    Data = response.Appointments.Select(a => new
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        DoctorName = a.DoctorName,
                        TimeSlot = a.TimeSlot,
                        Status = a.Status
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
} 