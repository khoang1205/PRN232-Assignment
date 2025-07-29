using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Notification;
using PRN232_Assignment.AppointmentService.Grpc.Data;
using PRN232_Assignment.AppointmentService.Grpc.Entities;
using User;
using AppointmentEntity = PRN232_Assignment.AppointmentService.Grpc.Entities.Appointment;

namespace PRN232_Assignment.AppointmentService.Grpc.Services
{
    public class AppointmentBusinessService : IAppointmentService
    {
        private readonly UserService.UserServiceClient _userClient;
        private readonly NotificationService.NotificationServiceClient _notificationClient;
        private readonly ApplicationDbContext _context;

        public AppointmentBusinessService(
            ApplicationDbContext context,
            UserService.UserServiceClient userClient,
            NotificationService.NotificationServiceClient notificationClient)
        {
            _context = context;
            _userClient = userClient;
            _notificationClient = notificationClient;
        }
       


        public async Task<string> CreateScheduleAsync(string doctorId, string dateStr)
		{
			var date = DateTime.Parse(dateStr);
			if (date.DayOfWeek == DayOfWeek.Sunday)
				throw new Exception("Không thể tạo lịch vào Chủ Nhật.");
            // 👇 Hardcode tạm GUID để test nếu chưa có DoctorId thật
          
            var schedule = new DailySchedule
			{
				Id = Guid.NewGuid(),
				//DoctorId = Guid.Parse(doctorId),
				DoctorId = doctorId,
				Date = date,
				TimeSlots = GenerateTimeSlots(date)
			};

			_context.DailySchedules.Add(schedule);
			await _context.SaveChangesAsync();

			return schedule.Id.ToString();
		}

		private List<TimeSlot> GenerateTimeSlots(DateTime date)
		{
			var slots = new List<TimeSlot>();
			DateTime[] periods = { date.Date.AddHours(8), date.Date.AddHours(13) };

			foreach (var start in periods)
			{
				for (int i = 0; i < 12; i++)
				{
					var slotStart = start.AddMinutes(i * 20);
					slots.Add(new TimeSlot
					{
						Id = Guid.NewGuid(),
						StartTime = slotStart,
						EndTime = slotStart.AddMinutes(20),
						Status = "Available"
					});
				}
			}

			return slots;
		}
		public async Task<List<TimeSlot>> GetAvailableSlotsAsync(string doctorId, string dateStr)
		{
			var date = DateTime.Parse(dateStr);
			var schedule = await _context.DailySchedules
				.Include(s => s.TimeSlots)
				.FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date.Date == date.Date);

			return schedule?.TimeSlots
				.Where(s => s.PatientId == null)
				.ToList() ?? new List<TimeSlot>();
		}
		public async Task<string> BookSlotAsync(string slotId, string patientId)
		{
			var slot = await _context.TimeSlots
				.Include(s => s.DailySchedule)
				.FirstOrDefaultAsync(s => s.Id == Guid.Parse(slotId));

			if (slot == null || slot.PatientId != null)
				throw new Exception("Slot đã được đặt.");

			var alreadyBooked = await _context.TimeSlots
				.AnyAsync(s =>
					s.PatientId == Guid.Parse(patientId) &&
					s.DailySchedule.DoctorId == slot.DailySchedule.DoctorId &&
					s.DailySchedule.Date == slot.DailySchedule.Date);

			if (alreadyBooked)
				throw new Exception("Mỗi bệnh nhân chỉ được đặt 1 slot/bác sĩ/ngày.");

			slot.PatientId = Guid.Parse(patientId);
			slot.Status = "Booked";
			await _context.SaveChangesAsync();
            var patient = await _userClient.GetUserAsync(new UserIdRequest
            {
                Id = patientId
            });
           
            var doctor = await _userClient.GetUserAsync(new UserIdRequest
            {
                Id = slot.DailySchedule.DoctorId
            });
            var patientName = patient?.Name ?? "bệnh nhân";
            var doctorName = doctor?.Name ?? "bác sĩ";
            //gui noti cho benh nhan
            await _notificationClient.SendNotificationAsync(new NotificationRequest
            {
                UserId = patientId,
                Type = "Appointment",
                Message = $"Bạn đã đặt lịch với {doctorName} lúc {slot.StartTime:HH:mm dd/MM}"
            });
           
            //gui noti cho bac si
            await _notificationClient.SendNotificationAsync(new NotificationRequest
            {
                UserId = slot.DailySchedule.DoctorId, 
                Type = "Appointment",
                Message = $"Bệnh nhân {patientName} đã đặt lịch khám lúc {slot.StartTime:HH:mm dd/MM}"
            });

            return slot.Id.ToString();
		}
        public async Task<string> GetDoctorIdFromSlotAsync(string slotId)
        {
            var slot = await _context.TimeSlots
                .Include(s => s.DailySchedule)
                .FirstOrDefaultAsync(s => s.Id.ToString() == slotId);

            if (slot == null || slot.DailySchedule == null)
                throw new Exception("Slot không hợp lệ hoặc không có lịch");

            return slot.DailySchedule.DoctorId.ToString(); // hoặc .Id nếu kiểu số
        }
    }
}
