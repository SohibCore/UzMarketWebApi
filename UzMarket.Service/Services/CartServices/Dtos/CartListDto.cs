namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos
{
    public class CartListDto
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public long UserId { get; set; }

        public ICollection<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
