using Article.Domain.MainModels.ArticleModels;

namespace Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories
{
    public interface ITechnicalRepository
    {
        ValueTask<ArticleModel?> GetArticleByIdAsync(Guid articleId);
        ValueTask<bool> SaveConclusionAsync(Guid articleId, string summary);
        ValueTask<bool> UpdateArticleStatusAsync(Guid articleId, ArticleStatus status);
    }
}
