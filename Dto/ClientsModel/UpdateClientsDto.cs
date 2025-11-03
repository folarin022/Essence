using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.ClientsModel
{
    public class UpdateClientsDto
    {
        [Required(ErrorMessage = "The FirstName field is required")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The OtherName field is required")]
        public string OtherName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Address field is required")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Email field is required")]
        public string Email {  get; set; } = string.Empty;
    }
}
 