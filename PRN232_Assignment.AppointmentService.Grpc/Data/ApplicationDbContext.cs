using Microsoft.EntityFrameworkCore;
using PRN232_Assignment.AppointmentService.Grpc.Entities;

namespace PRN232_Assignment.AppointmentService.Grpc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

      
        public DbSet<Entities.Appointment> Appointments { get; set; }
		public DbSet<DailySchedule> DailySchedules { get; set; }
		public DbSet<TimeSlot> TimeSlots { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
