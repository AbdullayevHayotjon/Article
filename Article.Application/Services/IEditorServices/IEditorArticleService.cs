using Article.Domain.MainModels.ArticleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Application.Services.IEditorServices
{
    public interface IEditorArticleService
    {
        Task<IEnumerable<ArticleDTO>> GetApprovedArticlesAsync(string category);
        Task<ArticleDTO?> GetArticleByIdAsync(string articleId);
        Task<bool> RejectArticleWithConclusionAsync(string articleId, string summary);
        Task<bool> ApproveArticleAsync(string articleId);
    }
}
