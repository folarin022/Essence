namespace EssenceShop.Dto.ClothesModel
{
    public class UpdateClothesDto
    {
        public string ClientsName { get; set; } = string.Empty;
        public string ClotheName { get; set; } = string.Empty;
        public string ClotheColour { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
