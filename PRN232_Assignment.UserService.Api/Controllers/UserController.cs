using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Service;
using PRN232_Assignment.UserService.Service.IService;
using PRN232_Assignment.UserService.Service.Models;

namespace PRN232_Assignment.UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _userService.Login(loginDto.Email, loginDto.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.RoleId != 1 && registerDto.RoleId != 2)
            {
                return BadRequest(new { message = "roleId must be 1: Doctor, 2: Patient" });
            }

            var success = await _userService.Register(registerDto);
            if (!success)
                return BadRequest(new { message = "Email already exists." });
            return Ok(new { message = "Registration successful." });
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
