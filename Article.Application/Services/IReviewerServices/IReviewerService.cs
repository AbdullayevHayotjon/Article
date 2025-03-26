
using Article.Domain.MainModels.UserModel;

namespace Article.Application.Services.IReviewerServices
{
    public interface IReviewerService
    {
        /// <summary>
        /// Tayinlangan maqolalarni olish
        /// </summary>
        Task<List<ArticleDto>> GetAssignedArticlesAsync(Guid reviewerId);

        /// <summary>
        /// Maqola tafsilotlarini olish
        /// </summary>
        Task<ArticleDto> GetArticleDetailsAsync(Guid articleId);

        /// <summary>
        /// Maqolaga taqriz yozish va muallifga jo‘natish
        /// </summary>
        Task SubmitReviewAsync(ReviewDto review);

        /// <summary>
        /// Umumiy tasdiqlangan maqolalarni saqlash
        /// </summary>
        Task SaveApprovedArticleAsync(Guid articleId);
    }

}

