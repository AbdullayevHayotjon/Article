using Article.Domain.HelpModels.ConclusionModel;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.TechnicalModels.ITechnicalRepositories;
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

        public async ValueTask<ArticleModel?> GetArticleByIdAsync(Guid articleId)
        {
            return await _context.ModelArticle
               .Include(a => a.User)
               .FirstOrDefaultAsync(a => a.Id == articleId);
        }

        public async ValueTask<bool> SaveConclusionAsync(Guid articleId, string summary)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null)
                return false;

            var conclusion = new Conclusion
            {
                ArticleId = articleId,
                Summary = summary
            };

            _context.Conclusions.Add(conclusion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> UpdateArticleStatusAsync(Guid articleId, ArticleStatus status)
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
