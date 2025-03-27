using Article.Application.Services.IArticleServices;
using Article.Application.Services.ICategoryServices;
using Article.Domain.HelpModels.ResubmitArticleRequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Article.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
  

    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        public ArticleController(IArticleService articleService, ICategoryService categoryService )
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("upload")]
        [Authorize(Roles = "User")]
        [SwaggerOperation(
            Summary = "maqolani yuklash qismi",
            Description = "maqola word formatda yuklanadi va unga sarlavha va kategory kiritiladi"
            )]
        public async Task<IActionResult> UploadArticle([FromForm] UploadArticleRequest request)
        {
            var userid= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var top = await _categoryService.GetCategoryByIdAsync(request.CategoryId);


            var article = await _articleService.UploadArticleAsync(request.File, request.Title, top.Name.ToString(),Guid.Parse(userid));
            return Ok(article);
        }


        [HttpGet("download/{articleId}")]
        [SwaggerOperation(
            Summary = "maqolani yuklash uchun link beradi",
            Description = "faqat maqola idsini kiritish yetarli"
            )]
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
        [Authorize(Roles = "User")]
        [SwaggerOperation(
            Summary = "maqolani qayta yuklash qismi",
            Description = "!!! maqola texnik taqrizchi yoki tahrirchi tomonidan qaytarilgan taqdirda qayta yuklash mumkin"
            )]
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
        [SwaggerOperation(
            Summary = "maqola va uning muallifi haqida ma'lumot qaytaradi",
            Description = "maqola id sini kiritish yetarli"
            )]
        public async Task<IActionResult> GetArticleById(Guid articleId)
        {
            var article = await _articleService.GetArticleByIdAsync(articleId);
            if (article == null)
                return NotFound(new { message = "Maqola topilmadi." });

            return Ok(article);
        }
    }
}
