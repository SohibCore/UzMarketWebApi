using MediatR;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ReviewDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Commands
{
    public record CreateReviewCommand(CreateReviewDlDto dto) : IRequest<int>;

    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public CreateReviewHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellation)
        {
            var review = new Review
            {
                ProductId = request.dto.ProductId,
                UserId = _service.UserId,
                RatingId = request.dto.RatingId,
                Comment = request.dto.Comment,

                CreatedAt = DateTime.UtcNow,
                CreateUserId = _service.UserId,
                StatusId = Core.StatusIdConst.CREATED
            };

            await _context.Reviews.AddAsync(review, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return review.Id;
        }
    }
}
