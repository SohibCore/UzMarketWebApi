namespace UzMarket.ServiceLayer.MediatorServices.AddressServices.Dtos
{
    public class AddressFilterDto
    {
        public int? Id { get; set; }
        public string? Region { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Street { get; set; } = null!;
        public string? PostalCode { get; set; } = null!;
    }
}
