namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class CreateCartDlDto
    {
        public long UserId { get; set; }
        public ICollection<CreateCartItemDlDto> Tables { get; set; } = new List<CreateCartItemDlDto>();
    }
}
