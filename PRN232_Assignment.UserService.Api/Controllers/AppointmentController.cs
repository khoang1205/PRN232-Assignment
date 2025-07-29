using Microsoft.AspNetCore.Mvc;
using Appointment;

namespace PRN232_Assignment.UserService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly Appointment.AppointmentService.AppointmentServiceClient _appointmentClient;

        public AppointmentController(Appointment.AppointmentService.AppointmentServiceClient appointmentClient)
        {
            _appointmentClient = appointmentClient;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId)
        {
            try
            {
                var request = new GetBookedTimeSlotsByPatientIdRequest
                {
                    PatientId = patientId
                };

                var response = await _appointmentClient.GetBookedTimeSlotsByPatientIdAsync(request);

                return Ok(new
                {
                    Success = true,
                    Data = response.Slots.Select(s => new
                    {
                        SlotId = s.SlotId,
                        DoctorId = s.DoctorId,
                        DoctorName = s.DoctorName,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Status = s.Status,
                        PatientId = s.PatientId
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Lỗi khi lấy appointments: {ex.Message}"
                });
            }
        }

    }
} 