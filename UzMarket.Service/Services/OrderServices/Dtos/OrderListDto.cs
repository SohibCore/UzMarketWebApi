namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos
{
    public class OrderListDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OrderDate { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingAddressId { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
