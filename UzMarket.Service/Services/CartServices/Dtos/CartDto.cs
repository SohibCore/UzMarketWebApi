namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos
{
    public class CartDto
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public long UserId { get; set; }
        public ICollection<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
