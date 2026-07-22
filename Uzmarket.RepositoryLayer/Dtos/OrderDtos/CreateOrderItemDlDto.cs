namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class CreateOrderItemDlDto
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
