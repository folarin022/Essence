using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace EssenceShop.Dto.ClothesModel
{
    public class CreateClothesDto
    {
        [Required(ErrorMessage = "The ClientsName field is required")]
        public string ClientsName { get; set; } = string.Empty;
        [Required(ErrorMessage = "The ClotheName field is required")]
        public string ClotheName {  get; set; } = string.Empty;
        [Required(ErrorMessage = "The ClotheColour field is required")]
        public string ClotheColour { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Quantity field is required")]
        public int Quantity { get; set; }
        public decimal AmountPaid { get; set; }

    }
}
