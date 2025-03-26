using Article.Domain.MainModels.ArticleModels;

namespace Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories
{
    public interface ITechnicalRepository
    {
        Task<List<ArticleDTO>> GetAllArticlesAsync();
        Task<ArticleDTO?> GetArticleByIdAsync(Guid articleId);
        Task<bool> SaveConclusionAsync(Guid articleId, string summary);
        Task<bool> UpdateArticleStatusAsync(Guid articleId, ArticleStatus status);
    }
}
