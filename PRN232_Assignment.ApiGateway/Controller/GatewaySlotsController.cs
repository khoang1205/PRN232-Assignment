using Microsoft.AspNetCore.Mvc;
using Appointment;
namespace PRN232_Assignment.ApiGateway.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewaySlotsController : ControllerBase
    {
        private readonly Appointment.AppointmentService.AppointmentServiceClient _client;

        public GatewaySlotsController(Appointment.AppointmentService.AppointmentServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetSlots([FromQuery] string doctorId, [FromQuery] string date)
        {
            var reply = await _client.GetAvailableSlotsAsync(new GetSlotsRequest
            {
                DoctorId = doctorId,
                Date = date
            });

            return Ok(new { slots = reply.Slots });
        }

        [HttpGet("booked")]
        public async Task<IActionResult> GetBookedSlotsByDoctor([FromQuery] string doctorId, [FromQuery] string date)
        {
            var reply = await _client.GetBookedSlotsByDoctorAsync(new GetBookedSlotsByDoctorRequest
            {
                DoctorId = doctorId,
                Date = date
            });

            return Ok(new { slots = reply.Slots });
        }
    }

}
