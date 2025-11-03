using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace EssenceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly EssenceDbContext _dbContext;
        private readonly JwtService _jwtService;
        private readonly ILogger<AdminController> _logger;

        // This key must be the SAME for both registration and login to verify hashes
        private readonly string _hmacKey = "ThisIsASecretKeyForHMAC";

        public AdminController(EssenceDbContext dbContext, JwtService jwtService, ILogger<AdminController> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _logger = logger;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            if (await _dbContext.Admins.AnyAsync(a => a.Username == request.Username))
                return BadRequest("Admin already exists.");

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));

            var admin = new Admin
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = "Admin"
            };

            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("New admin '{Username}' registered successfully at {Time}", request.Username, DateTime.UtcNow);
            return Ok("Admin registered successfully!");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (admin == null)
            {
                _logger.LogWarning("Login attempt failed: admin '{Username}' not found", request.Username);
                return Unauthorized("Admin not found.");
            }

            if (!VerifyPassword(request.Password, admin.PasswordHash))
            {
                _logger.LogWarning("Invalid password attempt for admin '{Username}'", request.Username);
                return Unauthorized("Invalid password.");
            }

            var token = _jwtService.GenerateToken(admin.Username, admin.Role);

            _logger.LogInformation("Admin '{Username}' logged in successfully at {Time}", request.Username, DateTime.UtcNow);
            return Ok(new
            {
                Message = "Login successful",
                Token = token
            });
        }



        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}
