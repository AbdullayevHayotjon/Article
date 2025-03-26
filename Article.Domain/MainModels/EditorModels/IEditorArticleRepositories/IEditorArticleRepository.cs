using Article.Domain.MainModels.ArticleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Domain.MainModels.EditorModels.IEditorArticleRepositories
{
    public interface IEditorArticleRepository
    {
        Task<IEnumerable<ArticleModel>> GetApprovedArticlesByCategoryAsync(string category);
        Task<ArticleModel?> GetArticleByIdAsync(string articleId);
        Task UpdateArticleAsync(ArticleModel article);
        Task SaveChangesAsync();
    }
}
