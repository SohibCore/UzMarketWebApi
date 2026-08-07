namespace UzMarket.RepositoryLayer.Dtos.AddressDtos
{
    public class UpdateAddressDlDto
    {
        public int Id { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
