namespace PRN232_Assignment.AppointmentService.Grpc.Entities
{
	public class DailySchedule
	{
		public Guid Id { get; set; }
		public Guid DoctorId { get; set; }
		public DateTime Date { get; set; }

		public ICollection<TimeSlot> TimeSlots { get; set; }
	}
}
