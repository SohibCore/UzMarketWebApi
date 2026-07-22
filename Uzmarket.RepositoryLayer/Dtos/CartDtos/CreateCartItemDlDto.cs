namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class CreateCartItemDlDto
    {
        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
