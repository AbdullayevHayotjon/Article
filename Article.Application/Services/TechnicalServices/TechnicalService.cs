using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Application.Services.TechnicalServices
{

    public class TechnicalService : ITechnicalService
    {
        private readonly ITechnicalRepository _technicalRepository;
        public TechnicalService(ITechnicalRepository technicalRepository)
        {
            _technicalRepository = technicalRepository;
        }
        public async ValueTask<IEnumerable<ArticleModel>> GetAllArticlesAsync()
        {
            return await _technicalRepository.GetAllAsync();
        }

        public async ValueTask<bool> ApproveArticleAsync(Guid articleId) =>
               await _technicalRepository.UpdateArticleStatusAsync(articleId, ArticleStatus.Approved);


        public async ValueTask<ArticleModel?> GetArticleByIdAsync(Guid articleId) =>
               await _technicalRepository.GetArticleByIdAsync(articleId);

        public async ValueTask<string?> ReadArticleAsync(Guid articleId)
        {
            var article = await _technicalRepository.GetArticleByIdAsync(articleId);
            if (article == null || string.IsNullOrWhiteSpace(article.FileUrl))
                return null;

            if (!File.Exists(article.FileUrl))
                return null;

            return await File.ReadAllTextAsync(article.FileUrl);
        }

        public async ValueTask<bool> RejectArticleAsync(Guid articleId, string summary)
        {
            bool saved = await SaveConclusionAsync(articleId, summary);
            if (!saved)
                return false;

            return await _technicalRepository.UpdateArticleStatusAsync(articleId, ArticleStatus.Rejected);
        }

        public async ValueTask<bool> SaveConclusionAsync(Guid articleId, string summary) =>
                await _technicalRepository.SaveConclusionAsync(articleId, summary);
    }
}
