namespace UzMarket.RepositoryLayer.Dtos.ProductDtos
{
    public class UpdateProductImageDlDto
    {
        public long Id { get; set; }
        public string? ImageUrl { get; set; }
        public Nullable<bool> MainPic { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<long> ProductId { get; set; }
    }
}
