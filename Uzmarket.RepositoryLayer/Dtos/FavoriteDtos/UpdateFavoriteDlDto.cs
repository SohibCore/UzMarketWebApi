namespace UzMarket.RepositoryLayer.Dtos.FavoriteDtos
{
    public class UpdateFavoriteDlDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }
}
