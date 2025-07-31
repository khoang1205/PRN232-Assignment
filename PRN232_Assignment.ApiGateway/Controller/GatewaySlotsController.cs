using Microsoft.AspNetCore.Mvc;
using Appointment;
using Grpc.Core;
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
        [HttpPost("book")]
        public async Task<IActionResult> BookSlot([FromBody] BookSlotRequest req)
        {
            try
            {
                var res = await _client.BookSlotAsync(req);
                return Ok(res);
            }
            catch (RpcException rpcEx)
            {
                // Đọc thông báo lỗi từ gRPC → trả lại frontend
                return BadRequest(new
                {
                    message = rpcEx.Status.Detail
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi không xác định: " + ex.Message
                });
            }
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] string patientId)
        {
            var res = await _client.GetBookedTimeSlotsByPatientIdAsync(
         new GetBookedTimeSlotsByPatientIdRequest { PatientId = patientId });
            return Ok(res);
        }
    }

}
