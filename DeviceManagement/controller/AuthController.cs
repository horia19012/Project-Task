using Microsoft.AspNetCore.Mvc;
using DeviceManagement.model;
using DeviceManagement.Config;
using Microsoft.EntityFrameworkCore;
namespace DeviceManagement.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SystemDbContext _context;

        public AuthController(SystemDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");
            return Ok(new { message = "Login successful." });
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return Conflict(new { message = "Email already used" });
            }
            var user = new User
            {
                Name = request.Name,
                Role = Role.user,
                Location = request.Location,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { message = "Registration successful." });
        }
    }
}