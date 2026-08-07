namespace UzMarket.RepositoryLayer.Dtos.ProductDtos
{
    public class UpdateProductDlDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> StockQuantity { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<long> SupplierId { get; set; }

        public ICollection<UpdateProductImageDlDto> Items { get; set; } = new List<UpdateProductImageDlDto>();
    }
}
