namespace EssenceShop.Dto.ClientsModel
{
    public class UpdateClientsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
 