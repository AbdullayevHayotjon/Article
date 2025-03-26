using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories;
using Article.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Article.Application.Services.TechnicalServices
{

    public class TechnicalService : ITechnicalService
    {
        private readonly ITechnicalRepository _technicalRepository;
        private readonly ApplicationDbContext _context;
        public TechnicalService(ITechnicalRepository technicalRepository, ApplicationDbContext applicationDb)
        {
            _technicalRepository = technicalRepository;
            _context = applicationDb;
        }
        public async Task<List<ArticleDTO>> GetAllArticlesAsync()
        {
            return await _technicalRepository.GetAllArticlesAsync();
        }

        public async Task<bool> ApproveArticleAsync(Guid articleId)
        {
            bool statusUpdated = await _technicalRepository.UpdateArticleStatusAsync(articleId, ArticleStatus.Approved);
            if (!statusUpdated)
                return false;

            var article = await _technicalRepository.GetArticleByIdAsync(articleId);
            if (article != null)
                await NotifyUserAboutApproval(article.User.Id, article.Title);

            return true;
        }
        private async Task NotifyUserAboutApproval(Guid userId, string articleTitle)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                string message = $"Sizning \"{articleTitle}\" maqolangiz tasdiqlandi!";
                Console.WriteLine($"✅ Foydalanuvchiga tasdiqlash xabari yuborildi: {user.Email} -> {message}");
            }
        }

        public async Task<ArticleDTO?> GetArticleByIdAsync(Guid articleId) =>
               await _technicalRepository.GetArticleByIdAsync(articleId);

        public async Task<string?> ReadArticleAsync(Guid articleId)
        {
            var article = await _technicalRepository.GetArticleByIdAsync(articleId);
            if (article == null || string.IsNullOrWhiteSpace(article.FileUrl))
                return null;

            if (!File.Exists(article.FileUrl))
                return null;

            return await File.ReadAllTextAsync(article.FileUrl);
        }

        public async Task<bool> RejectArticleAsync(Guid articleId, string summary)
        {
            bool saved = await SaveConclusionAsync(articleId, summary);
            if (!saved)
                return false;

            return await _technicalRepository.UpdateArticleStatusAsync(articleId, ArticleStatus.Rejected);
        }

        public async Task<bool> SaveConclusionAsync(Guid articleId, string summary) =>
                await _technicalRepository.SaveConclusionAsync(articleId, summary);
    }
}
