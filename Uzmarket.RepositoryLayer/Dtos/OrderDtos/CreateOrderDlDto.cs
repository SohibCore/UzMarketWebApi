namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class CreateOrderDlDto
    {
        public string OrderDate { get; set; } = null!;
        public int ShippingAddressId { get; set; }

        public ICollection<CreateOrderItemDlDto> Tables { get; set; } = new List<CreateOrderItemDlDto>();
    }
}
