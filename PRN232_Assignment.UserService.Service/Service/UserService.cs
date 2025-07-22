using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Repository.Repository;
using PRN232_Assignment.UserService.Service.IService;
using PRN232_Assignment.UserService.Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232_Assignment.UserService.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<LoginResponseDto> Login(string email, string password)
        {
            var account = await _repository.GetSingleByConditionAsynce(u => u.Email == email && u.Password == password);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            var token = GenerateJwtToken(account);

            return new LoginResponseDto
            {
                Token = token,
                RoleId = account.RoleId
            };
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return null;
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync(null, r => r.Role);
            return users.ToList();
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            var existingUser = await _repository.GetSingleByConditionAsynce(u => u.Email == registerDto.Email);
            if (existingUser != null)
                return false;

            var user = _mapper.Map<User>(registerDto);
            await _repository.AddAsync(user);
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim("id", user.Id.ToString()),
            new Claim("email", user.Email.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
