namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
