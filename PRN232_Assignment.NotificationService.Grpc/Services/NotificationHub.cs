
    using Microsoft.AspNetCore.SignalR;

    namespace PRN232_Assignment.NotificationServices.Grpc.Hubs
    {
        public class NotificationHub : Hub
        {
            public override async Task OnConnectedAsync()
            {
                var userId = Context.GetHttpContext().Request.Query["userId"];
                var role = Context.GetHttpContext().Request.Query["role"];

                await Groups.AddToGroupAsync(Context.ConnectionId, $"{role}-{userId}");
                await base.OnConnectedAsync();
            }
        }
    }

