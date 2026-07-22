namespace UzMarket.RepositoryLayer.Dtos.CartDtos
{
    public class UpdateCartDlDto
    {
        public long Id { get; set; }
        public int? StatusId { get; set; }
        public ICollection<UpdateCartItemDlDto> Tables { get; set; } = new List<UpdateCartItemDlDto>();
    }
}
