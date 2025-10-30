namespace EssenceShop.Dto.ClothesModel
{
    public class ClothesDto
    {
        public Guid Id { get; set; }
        public Guid ClotheId { get; set; } = Guid.NewGuid();
        public string ClotheName { get; set; } = string.Empty;
        public string ClotheColour { get; set; } = string.Empty;
        public  decimal PricePerClothe { get; set; } = 400m;
        public int Quantity { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountPaid { get; set; }
        public string Available { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
