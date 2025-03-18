using Article.Application.Services.IArticleServices;
using Article.Domain.HelpModels.ResubmitArticleRequestModel;
using Microsoft.AspNetCore.Mvc;

namespace Article.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost("upload")]
public async Task<IActionResult> UploadArticle([FromForm] UploadArticleRequest request)
{
    var article = await _articleService.UploadArticleAsync(request.File, request.Title, request.Category, request.UserId);
    return Ok(article);
}


        [HttpGet("download/{articleId}")]
        public async Task<IActionResult> DownloadArticle(Guid articleId)
        {
            try
            {
                var fileBytes = await _articleService.DownloadArticleAsync(articleId);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "article.docx");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("resubmit/{articleId}")]
        public async Task<IActionResult> ResubmitArticle([FromForm] ResubmitArticleRequest request)
        {
            try
            {
                var article = await _articleService.ResubmitArticleAsync(request.ArticleId, request.File);
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticleById(Guid articleId)
        {
            var article = await _articleService.GetArticleByIdAsync(articleId);
            if (article == null)
                return NotFound(new { message = "Maqola topilmadi." });

            return Ok(article);
        }
    }
}
