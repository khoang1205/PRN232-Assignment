using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Notification;
using PRN232_Assignment.NotificationServices.Grpc.Hubs;

namespace PRN232_Assignment.NotificationServices.Grpc.Services
{
    public class NotificationGrpcService : NotificationService.NotificationServiceBase
    {
        private readonly ILogger<NotificationGrpcService> _logger;
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationGrpcService(ILogger<NotificationGrpcService> logger, IHubContext<NotificationHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public override async Task<NotificationResponse> SendNotification(NotificationRequest request, ServerCallContext context)
        {
            _logger.LogInformation($" [Notification] To: {request.UserId}, Type: {request.Type}, Message: {request.Message}");

            // Gửi đến SignalR
            await _hub.Clients.Group($"{request.Type}-{request.UserId}")
                .SendAsync("ReceiveNotification", request.Message);

            // TODO: Có thể lưu vào DB hoặc gọi bên thứ 3 (email, push...)
            return new NotificationResponse { Status = "Delivered" };
        }

    }
}
