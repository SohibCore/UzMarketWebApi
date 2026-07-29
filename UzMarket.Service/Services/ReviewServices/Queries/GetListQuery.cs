using MediatR;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Services.ReviewServices.Dtos;
using UzMarket.ServiceLayer.Services.ReviewServices.Queries.QueryObjects;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Queries
{
    public record GetListQuery(ReviewFilterDto filter) : IRequest<List<ReviewListDto>>;

    public class GetListHandler : IRequestHandler<GetListQuery, List<ReviewListDto>>
    {
        private readonly AppDbContext _context;
        public GetListHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReviewListDto>> Handle(GetListQuery request, CancellationToken cancellation)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(x => x.StatusId != Core.StatusIdConst.DELETED)
                .Select(r => new ReviewListDto
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    RatingId = r.RatingId,
                    Comment = r.Comment,
                    StatusId = r.StatusId
                }).SortFilter(request.filter)
                .ToListAsync(cancellation);

            return reviews;
        }
    }
}
