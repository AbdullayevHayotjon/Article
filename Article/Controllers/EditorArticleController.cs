using Article.Application.Services.IEditorServices;
using Microsoft.AspNetCore.Mvc;

namespace Article.API.Controllers
{
    [Route("api/editor/articles")]
    [ApiController]
    public class EditorArticleController : ControllerBase
    {
        private readonly IEditorArticleService _articleService;

        public EditorArticleController(IEditorArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedArticles([FromQuery] string category)
        {
            var articles = await _articleService.GetApprovedArticlesAsync(category);
            return Ok(articles);
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticleById(string articleId)
        {
            var article = await _articleService.GetArticleByIdAsync(articleId);
            return article != null ? Ok(article) : NotFound();
        }

        [HttpPost("{articleId}/reject")]
        public async Task<IActionResult> RejectArticle(string articleId, [FromBody] string summary)
        {
            var result = await _articleService.RejectArticleWithConclusionAsync(articleId, summary);
            return result ? Ok() : NotFound();
        }

        [HttpPost("{articleId}/approve")]
        public async Task<IActionResult> ApproveArticle(string articleId)
        {
            var result = await _articleService.ApproveArticleAsync(articleId);
            return result ? Ok() : NotFound();
        }
    }
}
