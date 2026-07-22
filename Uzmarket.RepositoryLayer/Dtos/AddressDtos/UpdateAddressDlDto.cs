namespace UzMarket.RepositoryLayer.Dtos.AddressDtos
{
    public class UpdateAddressDlDto
    {
        public int Id { get; set; }
        public string? Region { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Street { get; set; } = null!;
        public string? PostalCode { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
