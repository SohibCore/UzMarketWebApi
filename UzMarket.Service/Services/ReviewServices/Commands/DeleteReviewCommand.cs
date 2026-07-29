using MediatR;
using UzMarket.Core;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.Security.AccountServices;

namespace UzMarket.ServiceLayer.Services.ReviewServices.Commands
{
    public record DeleteReviewCommand(int Id) : IRequest<bool>;

    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAccountService _service;
        public DeleteReviewHandler(AppDbContext context, IAccountService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellation)
        {
            var review = await _context.Reviews.SingleOrDefaultAsync(x => x.Id == request.Id && x.StatusId != StatusIdConst.DELETED && x.UserId == _service.UserId, cancellation);

            if (review == null)
                throw new NotFoundException($"Review not found : {request.Id}");

            review.StatusId = StatusIdConst.DELETED;
            review.ModifiedAt = DateTime.UtcNow;
            review.ModifiedUserId = _service.UserId;

            await _context.SaveChangesAsync(cancellation);
            return true;
        }
    }
}
