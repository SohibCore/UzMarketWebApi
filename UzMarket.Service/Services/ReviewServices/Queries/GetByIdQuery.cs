using MediatR;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.Services.ReviewServices.Dtos;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Queries
{
    public record GetByIdQuery(int Id) : IRequest<ReviewDto>;

    public class GetByIdHandler : IRequestHandler<GetByIdQuery, ReviewDto>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public GetByIdHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<ReviewDto> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var review = await _context.Reviews
                .AsNoTracking()
                .Where(x => x.Id == request.Id && x.StatusId != Core.StatusIdConst.DELETED)
                .Select(x => new ReviewDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    UserId = x.UserId,
                    RatingId = x.RatingId,
                    Comment = x.Comment,
                    StatusId = x.StatusId,
                }).FirstOrDefaultAsync(cancellation);

            if (review == null)
                throw new NotFoundException($"Review not found : {request.Id}");

            return review;
        }
    }
}
