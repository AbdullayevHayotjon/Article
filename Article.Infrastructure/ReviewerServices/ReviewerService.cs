
using Article.Application.Services.IReviewerServices;
using Article.Domain.HelpModels.ReviewModel;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.ReviewerServices
{
    public class ReviewerService : IReviewerService
    {
        private readonly ApplicationDbContext _context;

        public ReviewerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArticleDto>> GetAssignedArticlesAsync(Guid reviewerId)
        {
            return await _context.ModelArticle
                .Where(a => _context.Specializations
                    .Any(s => s.UserId == reviewerId && s.WorkerCategory == a.Category))
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Category = a.Category,
                    FileUrl = a.FileUrl
                }).ToListAsync();
        }

        public async Task<ArticleDto> GetArticleDetailsAsync(Guid articleId)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null) return null;

            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Category = article.Category,
                FileUrl = article.FileUrl
            };
        }

        public async Task SubmitReviewAsync(ReviewDto review)
        {
            var reviewModel = new Review
            {
                Id = Guid.NewGuid(),
                ArticleId = review.ArticleId,
                ReviewerId = review.ReviewerId,
                Comments = review.Comments,
                ReviewedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(reviewModel);
            await _context.SaveChangesAsync();
        }

        public async Task SaveApprovedArticleAsync(Guid articleId)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null) return;

            article.Status = ArticleStatus.Approved;
            await _context.SaveChangesAsync();
        }
    }

}
