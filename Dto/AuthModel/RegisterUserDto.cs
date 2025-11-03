using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.AuthModel
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;    }
}
