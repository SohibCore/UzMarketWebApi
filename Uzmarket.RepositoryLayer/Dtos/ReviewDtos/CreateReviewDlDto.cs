namespace UzMarket.RepositoryLayer.Dtos.ReviewDtos
{
    public class CreateReviewDlDto
    {
        public long ProductId { get; set; }
        public int RatingId { get; set; }
        public string? Comment { get; set; }

    }
}
