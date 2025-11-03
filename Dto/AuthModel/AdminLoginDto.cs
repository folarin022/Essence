using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.AuthModel
{
    public class AdminLoginDto
    {

        [MaxLength(50, ErrorMessage = "UserName must be at least 8 characters.")]
        public string Username { get; set; } = string.Empty;
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
