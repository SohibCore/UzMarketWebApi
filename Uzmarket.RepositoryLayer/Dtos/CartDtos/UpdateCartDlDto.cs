namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class UpdateCartDlDto
    {
        public long Id { get; set; }
        public int? StatusId { get; set; }
        public ICollection<UpdateCartItemDlDto> Items { get; set; } = new List<UpdateCartItemDlDto>();
    }
}
