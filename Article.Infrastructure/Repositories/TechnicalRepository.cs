using Article.Domain.HelpModels.ConclusionModel;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories;
using Article.Domain.MainModels.UserModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Infrastructure.Repositories
{
    public class TechnicalRepository : ITechnicalRepository
    {
        private readonly ApplicationDbContext _context;
        public TechnicalRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<ArticleDTO>> GetAllArticlesAsync()
        {
            var articles = await _context.ModelArticle
                .Include(a => a.User) // User ma’lumotlari bilan yuklaymiz
                .ToListAsync();

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
                    Id = a.UserId,
                    Firstname = a.User?.Firstname ?? "",
                    Lastname = a.User?.Lastname ?? "",
                    Email = a.User?.Email ?? "",
                    Username = a.User?.Username ?? ""
                }
            }).ToList();
        }


        public async Task<ArticleDTO?> GetArticleByIdAsync(Guid articleId)
        {
            var article = await _context.ModelArticle
                .Include(a => a.User) // User bilan bog‘laymiz
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null)
                return null;

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
                    Id = article.UserId,
                    Firstname = article.User?.Firstname ?? "",
                    Lastname = article.User?.Lastname ?? "",
                    Email = article.User?.Email ?? "",
                    Username = article.User?.Username ?? ""
                }
            };
        }


        public async Task<bool> SaveConclusionAsync(Guid articleId, string summary)
        {
            var article = await _context.ModelArticle
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null)
                return false;

            var conclusion = new Conclusion
            {
                ArticleId = articleId,
                Summary = summary,
                CreateDate = DateTime.UtcNow
            };

            _context.Conclusions.Add(conclusion);
            await _context.SaveChangesAsync();

            // Maqola egasiga xabar yuborish logikasi
            await NotifyUserAboutConclusion(article.UserId, article.Title, summary);

            return true;
        }
        private async Task NotifyUserAboutConclusion(Guid userId, string articleTitle, string summary)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                string message = $"Sizning \"{articleTitle}\" maqolangiz bo‘yicha xulosa mavjud: {summary}";
                Console.WriteLine($"📩 Foydalanuvchiga xabar yuborildi: {user.Email} -> {message}");
            }
        }
        public async Task<bool> UpdateArticleStatusAsync(Guid articleId, ArticleStatus status)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null)
                return false;

            article.Status = status;
            _context.ModelArticle.Update(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
