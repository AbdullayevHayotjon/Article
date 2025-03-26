using Article.Application.Services.IEditorServices;
using Article.Domain.HelpModels.ConclusionModel;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.EditorModels.IEditorArticleRepositories;
using Article.Domain.MainModels.UserModel;

namespace Article.Infrastructure.Services
{
    public class EditorArticleService : IEditorArticleService
    {
        private readonly IEditorArticleRepository _articleRepository;

        public EditorArticleService(IEditorArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<ArticleDTO>> GetApprovedArticlesAsync(string category)
        {
            var articles = await _articleRepository.GetApprovedArticlesByCategoryAsync(category);

            return articles.Select(a => new ArticleDTO
            {
                Id = a.Id.ToString(),
                Title = a.Title,
                Category = a.Category,
                ViewCount = a.ViewCount,
                DownloadCount = a.DownloadCount,
                FileUrl = a.FileUrl,
                PublishedDate = a.PublishedDate,
                Status = a.Status,
                User = new UserDTO
                {
                    Id = a.User.Id,
                    Firstname = a.User.Firstname
                }
            }).ToList();
        }

        public async Task<ArticleDTO?> GetArticleByIdAsync(string articleId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null) return null;

            return new ArticleDTO
            {
                Id = article.Id.ToString(),
                Title = article.Title,
                Category = article.Category,
                ViewCount = article.ViewCount,
                DownloadCount = article.DownloadCount,
                FileUrl = article.FileUrl,
                PublishedDate = article.PublishedDate,
                Status = article.Status,
                User = new UserDTO
                {
                    Id = article.User.Id,
                    Firstname = article.User.Firstname
                }
            };
        }

        public async Task<bool> RejectArticleWithConclusionAsync(string articleId, string summary)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null) return false;

            article.Status = ArticleStatus.Rejected;
            article.Conclusion = new Conclusion { Summary = summary };

            await _articleRepository.UpdateArticleAsync(article);
            await _articleRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveArticleAsync(string articleId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null) return false;

            article.Status = ArticleStatus.Completed;

            await _articleRepository.UpdateArticleAsync(article);
            await _articleRepository.SaveChangesAsync();

            return true;
        }
    }
}
