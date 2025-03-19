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
                var result = await _articleService.DownloadArticleAsync(articleId);
                if (!result.IsSuccess)
                    return NotFound(new { message = result.Error?.Message });

                return File(result.Value, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "article.docx");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpPut("resubmit/{articleId}")]
        public async Task<IActionResult> ResubmitArticle(Guid articleId, [FromForm] ResubmitArticleRequest request)
        {
            try
            {
                var result = await _articleService.ResubmitArticleAsync(articleId, request.File);
                return Ok(result);
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
