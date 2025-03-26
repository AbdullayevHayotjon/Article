using Article.Domain.MainModels.ArticleModels;

namespace Article.Application.Services.TechnicalServices
{
    public interface ITechnicalService
    {
        Task<List<ArticleDTO>> GetAllArticlesAsync();
        Task<ArticleDTO?> GetArticleByIdAsync(Guid articleId);
        Task<string?> ReadArticleAsync(Guid articleId);
        Task<bool> SaveConclusionAsync(Guid articleId, string summary);
        Task<bool> ApproveArticleAsync(Guid articleId);
        Task<bool> RejectArticleAsync(Guid articleId, string summary);
    }
}
