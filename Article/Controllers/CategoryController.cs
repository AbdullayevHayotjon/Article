using Article.Application.Services.ICategoryServices;
using Article.Domain.HelpModels.CategoryModel;
using Microsoft.AspNetCore.Mvc;

namespace Article.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Barcha kategoriyalarni olish
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Faqat kategoriyalarning nomlarini olish
        /// </summary>
        [HttpGet("names")]
        public async Task<IActionResult> GetCategoryNames()
        {
            var categoryNames = await _categoryService.GetCategoryNamesAsync();
            return Ok(categoryNames);
        }

        /// <summary>
        /// Kategoriya ID bo‘yicha olish
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound("Kategoriya topilmadi!");

            return Ok(category);
        }

        /// <summary>
        /// Yangi kategoriya qo‘shish
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = categoryName
            };
            await _categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Kategoriyani yangilash
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] Category category)
        {
            if (id != category.Id) return BadRequest("ID mos kelmadi!");

            await _categoryService.UpdateCategoryAsync(category);
            return NoContent();
        }

        /// <summary>
        /// Kategoriyani o‘chirish
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
