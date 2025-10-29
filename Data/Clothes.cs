namespace EssenceShop.Data
{
    public class Clothes
    {
        public Guid Id {  get; set; }
        public Guid ClothesId { get; set; } = Guid.NewGuid();
        public string ClientsName { get; set; } = string.Empty;
        public string ClotheName { get; set; } = string.Empty;
        public string ClotheColour { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal pricePerCloth { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal totalPrice { get; set; }
        public decimal Balance { get;set; }

        
    }
}
