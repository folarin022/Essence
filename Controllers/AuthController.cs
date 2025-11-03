using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EssenceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EssenceDbContext _dbContext;
        private readonly JwtService _jwtService;

        public AuthController(EssenceDbContext dbContext, JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
            };

            if (!ValidationService.IsValidEmail(request.Email))
                return BadRequest("Invalid email format.");

            if (!ValidationService.IsValidPassword(request.Password))
                return BadRequest("Invalid password format.");

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = $"{user.Role} registered successfully" });
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user.Username, user.Role);

            return Ok(new
            {
                message = "Login successful",
                token
            });
        }





        private string HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes("ThisIsASecretKeyForHMAC"); 
            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var key = Encoding.UTF8.GetBytes("ThisIsASecretKeyForHMAC"); 
            using var hmac = new HMACSHA256(key);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }


    }

}