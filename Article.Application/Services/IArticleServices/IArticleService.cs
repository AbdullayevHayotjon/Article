
using Article.Domain.Abstractions;
using Article.Domain.MainModels.ArticleModels;
using Microsoft.AspNetCore.Http;

namespace Article.Application.Services.IArticleServices
{
    public interface IArticleService
    {
        Task<Result<ArticleModel>> UploadArticleAsync(IFormFile file, string title, string category, Guid userId);
        Task<Result<byte[]>> DownloadArticleAsync(Guid articleId);
        Task<Result<ArticleModel>> ResubmitArticleAsync(Guid articleId, IFormFile file);
        Task<Result<ArticleModel?>> GetArticleByIdAsync(Guid articleId);
    }
}
