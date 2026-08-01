namespace UzMarket.ServiceLayer.MediatorServices.OrderServices.Dtos
{
    public class OrderFilterDto
    {
        public DateTime OrderDate { get; set; } 
        public decimal? TotalAmount { get; set; }
        public int? OrderStatusId { get; set; }
        public int? ShippingAddressId { get; set; }
    }
}
