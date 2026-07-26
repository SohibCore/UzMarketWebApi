namespace UzMarket.ServiceLayer.MediatorServices.CartServices.Dtos
{
    public class CartItemDto
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
