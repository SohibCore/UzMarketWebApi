using UzMarket.Core;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Dtos
{
    public class ReviewFilterDto
    {
        public int? Id { get; set; }
        public StatusIdConst? StatusId { get; set; }
    }
}
