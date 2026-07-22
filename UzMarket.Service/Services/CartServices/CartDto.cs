namespace UzMarket.ServiceLayer.Services.CartServices
{
    public class CartDto
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public long UserId { get; set; }
        public ICollection<CartItemDto> Tables { get; set; } = new List<CartItemDto>();
    }
}
