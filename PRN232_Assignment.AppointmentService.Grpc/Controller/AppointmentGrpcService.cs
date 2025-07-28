using Grpc.Core;
using Appointment;
using PRN232_Assignment.AppointmentService.Grpc.Services;

namespace PRN232_Assignment.AppointmentService.Grpc.Controller
{
    public class AppointmentGrpcService : Appointment.AppointmentService.AppointmentServiceBase

    {
        private readonly DoctorClient _doctorClient;
        private readonly IAppointmentService _service;

        public AppointmentGrpcService(IAppointmentService service, DoctorClient doctorClient)
        {
            _service = service;
            _doctorClient = doctorClient;
        }

        public override async Task<ScheduleResponse> CreateSchedule(CreateScheduleRequest request, ServerCallContext context)
        {
            try
            {
                var doctor = await _doctorClient.GetDoctorByIdAsync(request.DoctorId);
                if (doctor == null || !doctor.IsActive)
                    throw new RpcException(new Status(StatusCode.NotFound, "Bác sĩ không tồn tại hoặc đã nghỉ"));

                var scheduleId = await _service.CreateScheduleAsync(doctor.Id.ToString(), request.Date);

                return new ScheduleResponse
                {
                    ScheduleId = scheduleId,
                    Status = "Created"
                };
            }
            catch (RpcException rpcEx)
            {
                throw; // đã là lỗi gRPC → giữ nguyên
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CreateSchedule: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message)); // rõ ràng hơn cho Postman
            }
        }


        public override async Task<GetSlotsReply> GetAvailableSlots(GetSlotsRequest request, ServerCallContext context)
		{
			var slots = await _service.GetAvailableSlotsAsync(request.DoctorId, request.Date);
			var reply = new GetSlotsReply();
			reply.Slots.AddRange(slots.Select(s => new TimeSlotReply
			{
				SlotId = s.Id.ToString(),
				StartTime = s.StartTime.ToString("yyyy-MM-dd HH:mm"),
				EndTime = s.EndTime.ToString("yyyy-MM-dd HH:mm"),
				IsBooked = s.PatientId != null
			}));
			return reply;
		}

		public override async Task<AppointmentResponse> BookSlot(BookSlotRequest request, ServerCallContext context)
		{
			var id = await _service.BookSlotAsync(request.SlotId, request.PatientId);
			return new AppointmentResponse { AppointmentId = id, Status = "Booked" };
		}

	}
}
