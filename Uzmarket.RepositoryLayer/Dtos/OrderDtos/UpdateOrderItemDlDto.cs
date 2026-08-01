namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class UpdateOrderItemDlDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
