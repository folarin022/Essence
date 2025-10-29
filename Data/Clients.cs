namespace EssenceShop.Data
{
    public class Clients
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
