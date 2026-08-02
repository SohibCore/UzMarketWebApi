namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class CreateCartDlDto
    {
        public ICollection<CreateCartItemDlDto> Items { get; set; } = new List<CreateCartItemDlDto>();
    }
}
