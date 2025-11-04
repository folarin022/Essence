using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.AuthModel
{
    public class AdminLoginDto
    {

        [MaxLength(50, ErrorMessage = "UserName must be at least 50 characters.")]
        public string Username { get; set; } = string.Empty;
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
