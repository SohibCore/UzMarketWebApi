namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class UpdateCartItemDlDto
    {
        public long? Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
