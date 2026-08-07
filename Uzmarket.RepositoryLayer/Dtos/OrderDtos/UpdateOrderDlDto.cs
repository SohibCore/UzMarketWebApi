namespace UzMarket.RepositoryLayer.Dtos.OrderDtos
{
    public class UpdateOrderDlDto
    {
        public long? Id { get; set; }
        public Nullable<long> OrderStatusId { get; set; }
        public int ShippingAddressId { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<UpdateOrderItemDlDto> Tables { get; set; } = new List<UpdateOrderItemDlDto>();
    }
}
