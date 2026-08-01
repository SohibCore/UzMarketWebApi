using UzMarket.Core;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Dtos
{
    public class ReviewListDto
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public int? RatingId { get; set; }
        public string? Comment { get; set; }
        public StatusIdConst StatusId { get; set; }
    }
}
