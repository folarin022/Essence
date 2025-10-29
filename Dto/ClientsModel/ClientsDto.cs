namespace EssenceShop.Dto.ClientsModel
{
    public class ClientsDto
    {
        public Guid Id { get; set; }
        public Guid ClientsId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OtherName {  get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
