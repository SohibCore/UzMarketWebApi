namespace UzMarket.ServiceLayer.Services.OrderServices
{
    public class OrderListDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string OrderDate { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int OrderStatusId { get; set; }
        public int ShippingAddressId { get; set; }
    }
}
