namespace UzMarket.RepositoryLayer.Dtos.ReviewDtos
{
    public class UpdateReviewDlDto
    {
        public int Id { get; set; }
        public int? RatingId { get; set; }
        public string? Comment { get; set; }
    }
}
