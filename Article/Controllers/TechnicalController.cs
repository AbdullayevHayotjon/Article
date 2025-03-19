using Article.Application.Services.TechnicalServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("{articleId}")]
        public async ValueTask<IActionResult> GetArticleByIdAsync(Guid articleId)
        {
            var article = await _technicalService.GetArticleByIdAsync(articleId);
            if (article == null)
                return NotFound();
            return Ok(article);
        }
        [HttpGet("Read/{articleId}")]
        public async ValueTask<IActionResult> ReadArticleAsync(Guid articleId)
        {
            var article = await _technicalService.ReadArticleAsync(articleId);
            if (article == null)
                return NotFound();
            return Ok(article);
        }
        [HttpPost("SaveConclusion/{articleId}")]
        public async ValueTask<IActionResult> SaveConclusionAsync(Guid articleId, string summary)
        {
            bool saved = await _technicalService.SaveConclusionAsync(articleId, summary);
            if (!saved)
                return BadRequest();
            return Ok();
        }
        [HttpPost("Approve/{articleId}")]
        public async ValueTask<IActionResult> ApproveArticleAsync(Guid articleId)
        {
            bool approved = await _technicalService.ApproveArticleAsync(articleId);
            if (!approved)
                return BadRequest();
            return Ok();
        }
        [HttpPost("Reject/{articleId}")]
        public async ValueTask<IActionResult> RejectArticleAsync(Guid articleId, string summary)
        {
            bool rejected = await _technicalService.RejectArticleAsync(articleId, summary);
            if (!rejected)
                return BadRequest();
            return Ok();
        }
    }
}
