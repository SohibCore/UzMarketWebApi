using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.ReviewDtos;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Commands
{
    public record UpdateReviewCommand(UpdateReviewDlDto dto) : IRequest<bool>;

    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public UpdateReviewHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellation)
        {
            var review = await _context.Reviews.SingleOrDefaultAsync(x => x.Id == request.dto.Id && x.StatusId != Core.StatusIdConst.DELETED && x.UserId == _service.UserId, cancellation);

            if (review == null)
                throw new NotFoundException($"Review not found : {request.dto.Id}");

            review.RatingId = request.dto.RatingId;
            review.Comment = request.dto.Comment;

            review.ModifiedAt = DateTime.UtcNow;
            review.ModifiedUserId = _service.UserId;
            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
