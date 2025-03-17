
using Article.Domain.MainModels.ArticleModels;
using Microsoft.AspNetCore.Http;

namespace Article.Application.Services.IArticleServices
{
    public interface IArticleService
    {
        Task<ArticleModel> UploadArticleAsync(IFormFile file, string title, string authorName);
        Task<byte[]> DownloadArticleAsync(Guid articleId);
        Task<ArticleModel> ResubmitArticleAsync(Guid articleId, IFormFile file);
        Task<ArticleModel?> GetArticleByIdAsync(Guid articleId);
    }
}
