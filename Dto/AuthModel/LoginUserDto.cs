using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.AuthModel
{
    public class LoginUserDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;


        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
