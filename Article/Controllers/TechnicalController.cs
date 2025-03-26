using Article.Application.Services.TechnicalServices;
using Article.Domain.MainModels.TechnicalModels;
using Aspose.Words;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Article.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalController : ControllerBase
    {
        private readonly ITechnicalService _technicalService;
        public TechnicalController(ITechnicalService technicalService)
        {
            _technicalService = technicalService;
        }

        [HttpGet("AllArticles")]
        public async Task<IActionResult> GetAllArticlesAsync()
        {
            var articles = await _technicalService.GetAllArticlesAsync();

            if (articles == null || !articles.Any())
                return NotFound("Maqolalar mavjud emas.");

            return Ok(articles);
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetArticleByIdAsync(Guid articleId)
        {
            var article = await _technicalService.GetArticleByIdAsync(articleId);
            if (article == null)
                return NotFound();
            return Ok(article);
        }
        [HttpGet("view/{articleId}")]
        public async Task<IActionResult> ViewArticle(Guid articleId)
        {
            var article = await _technicalService.GetArticleByIdAsync(articleId);
            if (article == null || string.IsNullOrWhiteSpace(article.FileUrl))
                return NotFound("Maqola topilmadi.");

            if (!System.IO.File.Exists(article.FileUrl))
                return NotFound("Fayl mavjud emas.");

            Document doc = new Document(article.FileUrl);
            string htmlContent;
            using (MemoryStream stream = new MemoryStream())
            {
                doc.Save(stream, SaveFormat.Html);
                htmlContent = Encoding.UTF8.GetString(stream.ToArray());
            }

            return Content(htmlContent, "text/html");
        }
        [HttpGet("Read/{articleId}")]
        public async Task<IActionResult> ReadArticleAsync(Guid articleId)
        {
            var article = await _technicalService.ReadArticleAsync(articleId);
            if (article == null)
                return NotFound();
            return Ok(article);
        }
        [HttpPost("SaveConclusion/{articleId}")]
        public async Task<IActionResult> SaveConclusionAsync(Guid articleId, [FromBody] SaveConclusionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Summary))
                return BadRequest("Xulosa matni bo'sh bo'lmasligi kerak.");

            bool saved = await _technicalService.SaveConclusionAsync(articleId, request.Summary);
            if (!saved)
                return BadRequest("Xulosa saqlanmadi.");

            return Ok("Xulosa saqlandi.");
        }
        [HttpPost("Approve/{articleId}")]
        public async Task<IActionResult> ApproveArticleAsync(Guid articleId)
        {
            bool approved = await _technicalService.ApproveArticleAsync(articleId);
            if (!approved)
                return BadRequest();
            return Ok();
        }
        [HttpPost("Reject/{articleId}")]
        public async Task<IActionResult> RejectArticleAsync(Guid articleId, [FromBody] SaveConclusionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Summary))
                return BadRequest("Xulosa matni bo'sh bo'lmasligi kerak.");

            bool rejected = await _technicalService.RejectArticleAsync(articleId, request.Summary);
            if (!rejected)
                return BadRequest("Maqola rad etilmadi.");

            return Ok("Maqola rad etildi.");
        }

    }
}
