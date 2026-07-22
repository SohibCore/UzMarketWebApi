namespace UzMarket.RepositoryLayer.Dtos.AddressDtos
{
    public class CreateAddressDlDto
    {
        public string Region { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
