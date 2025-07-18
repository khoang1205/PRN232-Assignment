    using System;
    using System.Threading.Tasks;
    using appointment_service.Data;
    using appointment_service.Entities;
    using User;
    using Notification;
using AppointmentEntity = appointment_service.Entities.Appointment;
namespace appointment_service.Services
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

            public async Task<string> CreateAsync(string doctorId, string patientId, string timeSlot)
            {
                // Gọi user_service để lấy thông tin bác sĩ và bệnh nhân
                var doctor = await _userClient.GetUserAsync(new UserIdRequest { Id = doctorId });
                var patient = await _userClient.GetUserAsync(new UserIdRequest { Id = patientId });

                if (doctor == null || doctor.Role != "Doctor")
                    throw new Exception("Invalid Doctor");

                if (patient == null || patient.Role != "Patient")
                    throw new Exception("Invalid Patient");

                // Tạo và lưu lịch hẹn
                var appointment = new AppointmentEntity
                {
                    Id = Guid.NewGuid(),
                    DoctorId = Guid.Parse(doctorId),
                    PatientId = Guid.Parse(patientId),
                    TimeSlot = DateTime.Parse(timeSlot),                
                    Status = "Pending"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Gửi thông báo cho bác sĩ
                await _notificationClient.SendNotificationAsync(new NotificationRequest
                {
                    UserId = doctorId,
                    Type = "AppointmentCreated",
                    Message = $"Bạn có lịch hẹn mới với bệnh nhân {patient.Name} vào lúc {appointment.TimeSlot:dd/MM/yyyy HH:mm}"
                });

                return appointment.Id.ToString();
            }
        }
    }
