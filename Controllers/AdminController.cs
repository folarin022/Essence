using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly string _hmacKey = "ThisIsASecretKeyForHMAC"; // Must be EXACT same in Register + Verify

        public AdminController(EssenceDbContext dbContext, JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminLoginDto request)
        {
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

            return Ok("Admin registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto request)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Username == request.Username);
            if (admin == null)
                return Unauthorized("Admin not found.");

            if (!VerifyPassword(request.Password, admin.PasswordHash))
                return Unauthorized("Invalid password.");

            var token = _jwtService.GenerateToken(admin.Username, admin.Role);
            return Ok(new { Message = "Login successful", Token = token });
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}
