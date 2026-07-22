namespace UzMarket.RepositoryLayer.Dtos.ProductDtos
{
    public class CreateProductDlDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public long CategoryId { get; set; }

        public ICollection<CreateProductImageDlDto> Tables { get; set; } = new List<CreateProductImageDlDto>();
    }
}
