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
    public class AuthController : ControllerBase
    {
        private readonly EssenceDbContext _dbContext;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private const string HmacKey = "ThisIsASecretKeyForHMAC"; 

        public AuthController(EssenceDbContext dbContext, JwtService jwtService, ILogger<AuthController> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                    return BadRequest("Email already exists.");

                if (!ValidationService.IsValidEmail(request.Email))
                    return BadRequest("Invalid email format.");

                if (!ValidationService.IsValidPassword(request.Password))
                    return BadRequest("Password must contain lowercase letters, numbers, and a symbol.");

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    Role = "Client"
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("New user '{Email}' registered successfully at {Time}", user.Email, DateTime.UtcNow);
                return Ok(new { message = $"{user.Role} registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for {Email}", request.Email);
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: user '{Email}' not found", request.Email);
                    return Unauthorized("Invalid credentials.");
                }

                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password attempt for user '{Email}'", request.Email);
                    return Unauthorized("Invalid credentials.");
                }

                var token = _jwtService.GenerateToken(user.Username, user.Role);

                _logger.LogInformation("User '{Email}' logged in successfully at {Time}", user.Email, DateTime.UtcNow);

                return Ok(new
                {
                    message = "Login successful",
                    token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", request.Email);
                return StatusCode(500, "An error occurred while logging in.");
            }
        }



        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(HmacKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }


        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(HmacKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}
