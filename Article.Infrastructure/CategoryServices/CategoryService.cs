using Article.Application.Services.ICategoryServices;
using Article.Domain.HelpModels.CategoryModel;
using Microsoft.EntityFrameworkCore;


namespace Article.Infrastructure.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categorys.ToListAsync();
        }

        public async Task<List<string>> GetCategoryNamesAsync()
        {
            return await _context.Categorys
                .Select(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categorys.FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            category.Id = Guid.NewGuid();
            await _context.Categorys.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categorys.FindAsync(category.Id);
            if (existingCategory == null) return;

            existingCategory.Name = category.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categorys.FindAsync(id);
            if (category == null) return;

            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
