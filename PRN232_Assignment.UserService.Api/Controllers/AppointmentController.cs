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
                var request = new GetAppointmentsByPatientIdRequest
                {
                    PatientId = patientId
                };

                var response = await _appointmentClient.GetAppointmentsByPatientIdAsync(request);

                return Ok(new
                {
                    Success = true,
                    Message = "Lấy danh sách appointments thành công",
                    Data = response.Appointments.Select(a => new
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        DoctorName = a.DoctorName,
                        TimeSlot = a.TimeSlot,
                        Status = a.Status
                    }).ToList(),
                    TotalCount = response.Appointments.Count
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

        [HttpGet("patient/{patientId}/summary")]
        public async Task<IActionResult> GetAppointmentSummary(string patientId)
        {
            try
            {
                var request = new GetAppointmentsByPatientIdRequest
                {
                    PatientId = patientId
                };

                var response = await _appointmentClient.GetAppointmentsByPatientIdAsync(request);

                var summary = new
                {
                    PatientId = patientId,
                    TotalAppointments = response.Appointments.Count,
                    PendingAppointments = response.Appointments.Count(a => a.Status == "Pending"),
                    CompletedAppointments = response.Appointments.Count(a => a.Status == "Completed"),
                    CancelledAppointments = response.Appointments.Count(a => a.Status == "Cancelled"),
                    UpcomingAppointments = response.Appointments
                        .Where(a => DateTime.Parse(a.TimeSlot) > DateTime.Now)
                        .OrderBy(a => DateTime.Parse(a.TimeSlot))
                        .Take(5)
                        .Select(a => new
                        {
                            AppointmentId = a.AppointmentId,
                            DoctorName = a.DoctorName,
                            TimeSlot = a.TimeSlot,
                            Status = a.Status
                        }).ToList()
                };

                return Ok(new
                {
                    Success = true,
                    Message = "Lấy thống kê appointments thành công",
                    Data = summary
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Lỗi khi lấy thống kê appointments: {ex.Message}"
                });
            }
        }
    }
} 