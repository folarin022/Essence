namespace EssenceShop.Dto.ClothesModel
{
    public class ClothesAmount
    {
        public decimal PricePerCloth { get; set; }

        public decimal CalculateTotal(int quantity)
        {
            return PricePerCloth * quantity;
        }
    }
}
