namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class UpdateOrderItemDlDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
