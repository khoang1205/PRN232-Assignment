using Grpc.Core;
using Appointment;
using PRN232_Assignment.AppointmentService.Grpc.Services;

namespace PRN232_Assignment.AppointmentService.Grpc.Controller
{
    public class AppointmentGrpcService : Appointment.AppointmentService.AppointmentServiceBase

    {
        private readonly IAppointmentService _service;

        public AppointmentGrpcService(IAppointmentService service)
        {
            _service = service;
        }

		public override async Task<ScheduleResponse> CreateSchedule(CreateScheduleRequest request, ServerCallContext context)
		{
			var id = await _service.CreateScheduleAsync(request.DoctorId, request.Date);
			return new ScheduleResponse { ScheduleId = id, Status = "Created" };
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
