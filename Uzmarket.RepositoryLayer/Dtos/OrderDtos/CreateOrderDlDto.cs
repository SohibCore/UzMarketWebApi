namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class CreateOrderDlDto
    {
        public DateTime OrderDate { get; set; } 
        public int ShippingAddressId { get; set; }

        public ICollection<CreateOrderItemDlDto> Items { get; set; } = new List<CreateOrderItemDlDto>();
    }
}
