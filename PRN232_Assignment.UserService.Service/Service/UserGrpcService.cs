using Grpc.Core;
using User;
using PRN232_Assignment.UserService.Service.IService;

namespace PRN232_Assignment.UserService.Api.Services
{
    public class UserGrpcService : User.UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly DoctorClient _doctorClient;

        public UserGrpcService(IUserService userService, DoctorClient doctorClient)
        {
            _userService = userService;
            _doctorClient = doctorClient;
        }

        public override async Task<UserResponse> GetUser(UserIdRequest request, ServerCallContext context)
        {
            try
            {
                // Nếu là bệnh nhân (GUID)
                if (Guid.TryParse(request.Id, out Guid userId))
                {
                    var user = await _userService.GetByIdAsync(userId);
                    if (user != null)
                    {
                        return new UserResponse
                        {
                            Id = user.Id.ToString(),
                            Name = user.Email,
                            Role = user.RoleId == 1 ? "Doctor" : "Patient"
                        };
                    }
                }

                // Nếu là bác sĩ (string Id từ MongoDB)
                var doctor = await _doctorClient.GetDoctorByIdAsync(request.Id);
                if (doctor != null)
                {
                    return new UserResponse
                    {
                        Id = doctor.Id,
                        Name = doctor.FullName,
                        Role = "Doctor"
                    };
                }

                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[gRPC] Error in GetUser: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, $"Internal error: {ex.Message}"));
            }
        }


    }
}
