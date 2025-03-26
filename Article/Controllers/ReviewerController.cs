using Article.Application.Services.IReviewerServices;
using Article.Domain.MainModels.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Article.Api.Controllers
{
    [Route("api/reviewer")]
    [ApiController]
    [Authorize(Roles = "Reviewer")]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [HttpGet("assigned-articles/{reviewerId}")]
        public async Task<IActionResult> GetAssignedArticles(Guid reviewerId)
        {
            var articles = await _reviewerService.GetAssignedArticlesAsync(reviewerId);
            return Ok(articles);
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetArticleDetails(Guid articleId)
        {
            var article = await _reviewerService.GetArticleDetailsAsync(articleId);
            if (article == null)
                return NotFound("Maqola topilmadi!");

            return Ok(article);
        }

        [HttpPost("submit-review")]
        public async Task<IActionResult> SubmitReview(string comment, Guid articleId)
        {
            var userid= User.FindFirstValue(ClaimTypes.NameIdentifier);
            ReviewDto reviewDto = new ReviewDto()
            {
                Id=Guid.NewGuid(),
                ArticleId=articleId,
                ReviewerId= Guid.Parse(userid),
                Comments=comment,
                ReviewedAt=DateTime.UtcNow
            };
            await _reviewerService.SubmitReviewAsync(reviewDto);
            return Ok("Taqriz muvaffaqiyatli saqlandi!");
        }

        [HttpPost("approve-article/{articleId}")]
        public async Task<IActionResult> ApproveArticle(Guid articleId)
        {
            await _reviewerService.SaveApprovedArticleAsync(articleId);
            return Ok("Maqola tasdiqlandi!");
        }
    }
}
