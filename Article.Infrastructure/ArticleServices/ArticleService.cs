using Article.Application.Services.IArticleServices;
using Article.Domain.Abstractions;
using Article.Domain.MainModels.ArticleModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.ArticleServices
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadPath = "wwwroot/uploads";

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ArticleModel>> UploadArticleAsync(IFormFile file, string title, string category, Guid userId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Fayl yuklanmadi.");

                if (Path.GetExtension(file.FileName).ToLower() != ".docx")
                    throw new ArgumentException("Faqat .docx formatdagi fayllarni yuklash mumkin.");

                if (!Directory.Exists(_uploadPath))
                    Directory.CreateDirectory(_uploadPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(_uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var article = new ArticleModel
                {
                    Title = title,
                    Category = category,
                    FileUrl = filePath,
                    UserId = userId,
                    Status = ArticleStatus.Sent,
                    PublishedDate = DateTime.UtcNow
                };

                _context.ModelArticle.Add(article);
                await _context.SaveChangesAsync();

                return Result<ArticleModel>.Success(article);
            }
            catch(Exception ex)
            {
                var top = new Error("222",ex.Message);
                return Result<ArticleModel>.Failure(top);
            }
        }

        public async Task<Result<byte[]>> DownloadArticleAsync(Guid articleId)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null || string.IsNullOrEmpty(article.FileUrl) || !File.Exists(article.FileUrl))
                throw new FileNotFoundException("Fayl topilmadi.");

            var fileBytes = await File.ReadAllBytesAsync(article.FileUrl);
            return Result<byte[]>.Success(fileBytes);
        }


        public async Task<Result<ArticleModel>> ResubmitArticleAsync(Guid articleId, IFormFile file)
        {
            var article = await _context.ModelArticle.FindAsync(articleId);
            if (article == null)
                throw new ArgumentException("Maqola topilmadi.");

            if (article.Status != ArticleStatus.Rejected)
                throw new ArgumentException("Faqat rad etilgan maqolalar qayta yuklanishi mumkin.");

            if (file == null || file.Length == 0)
                throw new ArgumentException("Fayl yuklanmadi.");

            if (Path.GetExtension(file.FileName).ToLower() != ".docx")
                throw new ArgumentException("Faqat .docx formatdagi fayllarni yuklash mumkin.");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(article.FileUrl) && File.Exists(article.FileUrl))
                File.Delete(article.FileUrl);

            article.FileUrl = filePath;
            article.Status = ArticleStatus.Pending;
            article.PublishedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<ArticleModel>.Success(article);
        }

        public async Task<Result<ArticleModel?>> GetArticleByIdAsync(Guid articleId)
        {
            var top= await _context.ModelArticle.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == articleId);
            return Result<ArticleModel>.Success(top);
        }
    }
}
