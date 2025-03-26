using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.EditorModels.IEditorArticleRepositories;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.Repositories
{
    public class EditorArticleRepository : IEditorArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public EditorArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleModel>> GetApprovedArticlesByCategoryAsync(string category)
        {
            return await _context.ModelArticle
                .Include(a => a.User)
                .Where(a => a.Category == category && a.Status == ArticleStatus.Approved)
                .ToListAsync();
        }

        public async Task<ArticleModel?> GetArticleByIdAsync(string articleId)
        {
            return await _context.ModelArticle
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id.ToString() == articleId);
        }

        public async Task UpdateArticleAsync(ArticleModel article)
        {
            _context.ModelArticle.Update(article);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
