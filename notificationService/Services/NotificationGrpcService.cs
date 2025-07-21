using Grpc.Core;
using Notification; 

namespace PRN232.Assignment.NotificationService
{
    public class NotificationGrpcService : NotificationService.NotificationServiceBase
    {
        private readonly ILogger<NotificationGrpcService> _logger;

        public NotificationGrpcService(ILogger<NotificationGrpcService> logger)
        {
            _logger = logger;
        }

        public override Task<NotificationResponse> SendNotification(NotificationRequest request, ServerCallContext context)
        {
            // In ra console để mock thông báo
            _logger.LogInformation($" [Notification] To: {request.UserId}, Type: {request.Type}, Message: {request.Message}");

            return Task.FromResult(new NotificationResponse
            {
                Status = "Delivered"
            });
        }
    }
}
