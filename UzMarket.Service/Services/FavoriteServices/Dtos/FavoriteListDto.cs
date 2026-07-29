namespace UzMarket.ServiceLayer.Services.FavoriteServices.Dtos
{
    public class FavoriteListDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
