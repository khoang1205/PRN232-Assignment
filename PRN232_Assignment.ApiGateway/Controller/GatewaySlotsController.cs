using Microsoft.AspNetCore.Mvc;
using Appointment;
using Grpc.Core;
namespace PRN232_Assignment.ApiGateway.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewaySlotsController : ControllerBase
    {
        private readonly AppointmentService.AppointmentServiceClient _client;

        public GatewaySlotsController(AppointmentService.AppointmentServiceClient client)
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

        [HttpGet("booked-by-doctor")]
        public async Task<IActionResult> GetBookedSlotsByDoctor([FromQuery] string doctorId, [FromQuery] string date)
        {
            if (string.IsNullOrEmpty(doctorId) || string.IsNullOrEmpty(date))
                return BadRequest(new { message = "doctorId và date là bắt buộc" });

            try
            {
                var reply = await _client.GetBookedSlotsByDoctorAsync(new GetBookedSlotsByDoctorRequest
                {
                    DoctorId = doctorId,
                    Date = date
                });

                return Ok(new { success = true, slots = reply.Slots });
            }
            catch (RpcException ex)
            {
                return StatusCode(500, new { success = false, message = ex.Status.Detail });
            }
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

        [HttpPost("create-schedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest req)
        {
            if (string.IsNullOrEmpty(req.DoctorId) || string.IsNullOrEmpty(req.Date))
            {
                return BadRequest(new { message = "doctorId và date là bắt buộc" });
            }

            try
            {
                var reply = await _client.CreateScheduleAsync(req);
                return Ok(new { success = true, scheduleId = reply.ScheduleId });
            }
            catch (RpcException rpcEx)
            {
                return BadRequest(new { success = false, message = rpcEx.Status.Detail });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

    }

}
