namespace UzMarket.ServiceLayer.MediatorServices.ProductServices.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public long SupplierId { get; set; }

        public ICollection<ProductImageDto> Tables { get; set; } = new List<ProductImageDto>();
    }
}
