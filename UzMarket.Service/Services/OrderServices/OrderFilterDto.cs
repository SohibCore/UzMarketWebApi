namespace UzMarket.ServiceLayer.Services.OrderServices
{
    public class OrderFilterDto
    {
        public string? OrderDate { get; set; } = null!;
        public decimal? TotalAmount { get; set; }
        public int? OrderStatusId { get; set; }
        public int? ShippingAddressId { get; set; }
    }
}
