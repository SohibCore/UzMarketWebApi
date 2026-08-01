namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime OrderDate { get; set; } 
        public decimal TotalAmount { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingAddressId { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
