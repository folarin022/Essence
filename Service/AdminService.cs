using EssenceShop.Context;
using EssenceShop.Controllers;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Repositries;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;
using EssenceShop.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EssenceShop.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepositries _adminRepository;
        private readonly ILogger<AdminService> _logger;
        private readonly JwtService _jwtService;
        private readonly EssenceDbContext _dbContext;

        private readonly string _hmacKey = "ThisIsASecretKeyForHMAC";

        public AdminService(EssenceDbContext dbContext, JwtService jwtService, ILogger<AdminService> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _logger = logger;
        }


        public AdminService(IAdminRepository adminRepository, ILogger<AdminService> logger, EssenceDbContext dbContext)
        {
            this._adminRepository = (IAdminRepositries)adminRepository;
            this._logger = logger;
            this._dbContext = dbContext;
        }
        public async Task<BaseResponse<bool>> Login(AdminLoginDto request, CancellationToken cancellationToken)
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

        private BaseResponse<bool> Ok(object value)
        {
            throw new NotImplementedException();
        }

        private BaseResponse<bool> Unauthorized(string v)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<bool>> RegisterAdmin(AdminLoginDto request, CancellationToken cancellationToken)
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
                Email = request.Email,
                Role = "Admin"
            };

            if (!ValidationService.IsValidEmail(request.Email))
                return BadRequest("Invalid email format.");

            if (!ValidationService.IsValidPassword(request.Password))
                return BadRequest("Password must contain lowercase letters, numbers, and a symbol.");

            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("New admin '{Username}' registered successfully at {Time}", request.Username, DateTime.UtcNow);
            return Ok("Admin registered successfully!");
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }

        private BaseResponse<bool> BadRequest(string v)
        {
            throw new NotImplementedException();
        }
    }
}
