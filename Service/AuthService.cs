using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;
using EssenceShop.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EssenceShop.Service
{
    public class AuthService : IAuthService
    {

        private readonly IAuthRepositries _authRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtService _jwtService;
        private readonly EssenceDbContext _dbContext;

        private readonly string _hmacKey = "ThisIsASecretKeyForHMAC";

        public AuthService(EssenceDbContext dbContext, JwtService jwtService, ILogger<AuthService> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> LoginClients(LoginUserDto request, CancellationToken cancellationToken)
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

        
        public async Task<BaseResponse<bool>> RegisterClients(RegisterUserDto request, CancellationToken cancellationToken)
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

        private BaseResponse<bool> BadRequest(string v)
        {
            throw new NotImplementedException();
        }

        private BaseResponse<bool> StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        private BaseResponse<bool> Ok(object value)
        {
            throw new NotImplementedException();
        }

        private BaseResponse<bool> Unauthorized(string v)
        {
            throw new NotImplementedException();
        }


        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

    }
}
