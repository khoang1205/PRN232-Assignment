namespace PRN232_Assignment.AppointmentService.Grpc.Entities
{
	public class TimeSlot
	{
		public Guid Id { get; set; }
		public Guid DailyScheduleId { get; set; }
		public DailySchedule DailySchedule { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public Guid? PatientId { get; set; }
		public string Status { get; set; } = "Available"; // or "Booked"
	}
}
