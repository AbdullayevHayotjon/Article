using Article.Domain.MainModels.ArticleModels;

namespace Article.Application.Services.TechnicalServices
{
    public interface ITechnicalService
    {
        ValueTask<ArticleModel?> GetArticleByIdAsync(Guid articleId);
        ValueTask<string?> ReadArticleAsync(Guid articleId);
        ValueTask<bool> SaveConclusionAsync(Guid articleId, string summary);
        ValueTask<bool> ApproveArticleAsync(Guid articleId);
        ValueTask<bool> RejectArticleAsync(Guid articleId, string summary);
    }
}
