using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using User;

namespace user_service.Services
{
    public class UserGrpcService : User.UserService.UserServiceBase
    {
        private readonly IUserService _service;

        public UserGrpcService(IUserService service)
        {
            _service = service;
        }

        public override async Task<UserResponse> GetUser(UserIdRequest request, ServerCallContext context)
        {
            var user = await _service.GetByIdAsync(Guid.Parse(request.Id));
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            return new UserResponse
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Role = user.Role
            };
        }
    }
}

