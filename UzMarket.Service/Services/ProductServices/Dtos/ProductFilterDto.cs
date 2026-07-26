namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos
{
    public class ProductFilterDto
    {
        public string? Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public decimal? Price { get; set; }
    }
}
