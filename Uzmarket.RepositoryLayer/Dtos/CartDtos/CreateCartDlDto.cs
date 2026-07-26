namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class CreateCartDlDto
    {
        public long UserId { get; set; }
        public ICollection<CreateCartItemDlDto> Items { get; set; } = new List<CreateCartItemDlDto>();
    }
}
