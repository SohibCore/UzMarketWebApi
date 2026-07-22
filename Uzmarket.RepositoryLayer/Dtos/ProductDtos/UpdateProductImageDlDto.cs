namespace UzMarket.RepositoryLayer.Dtos.ProductDtos
{
    public class UpdateProductImageDlDto
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool MainPic { get; set; }
        public int SortOrder { get; set; }
        public long ProductId { get; set; }
    }
}
