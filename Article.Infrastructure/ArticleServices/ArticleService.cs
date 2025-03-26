using Article.Application.Services.IArticleServices;
using Article.Domain.Abstractions;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<Result<ArticleModel>> UploadArticleAsync(IFormFile file, string title, string category,Guid userId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Result<ArticleModel>.Failure(UserError.checkFileUpload);

                if (Path.GetExtension(file.FileName).ToLower() != ".docx")
                    return Result<ArticleModel>.Failure(UserError.ErrodFormatFile);

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
               
                return Result<ArticleModel>.Failure(new Error("UploadArticleAsync da xatolik",ex.Message));
            }
        }

        public async Task<Result<byte[]>> DownloadArticleAsync(Guid articleId)
        {
            try
            {
                var article = await _context.ModelArticle.FindAsync(articleId);
                if (article == null || string.IsNullOrEmpty(article.FileUrl) || !File.Exists(article.FileUrl))
                    return Result<byte[]>.Failure(new Error("FileNotFound", "Fayl topilmadi yoki yo‘q."));

                var fileBytes = await File.ReadAllBytesAsync(article.FileUrl);
                return Result<byte[]>.Success(fileBytes);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure(new Error("DownloadError", $"Xatolik yuz berdi: {ex.Message}"));
            }
        }


        public async Task<Result<ArticleModel>> ResubmitArticleAsync(Guid articleId, IFormFile file)
        {
            try
            {
                var article = await _context.ModelArticle.FindAsync(articleId);
                if (article == null)
                    return Result<ArticleModel>.Failure(UserError.ErrorArticleDontFind);

                if (article.Status != ArticleStatus.Rejected)
                    return Result<ArticleModel>.Failure(UserError.rejectedError);


                if (file == null || file.Length == 0)
                    return Result<ArticleModel>.Failure(UserError.checkFileUpload);

                if (Path.GetExtension(file.FileName).ToLower() != ".docx")
                    return Result<ArticleModel>.Failure(UserError.ErrodFormatFile);

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
            catch(Exception ex)
            {
                return Result<ArticleModel>.Failure(new Error("ResubmitArticleAsync da xatolik", ex.Message));

            }
        }

        public async Task<Result<ArticleModel>> GetArticleByIdAsync(Guid articleId)
        {
            try
            {
                var top = await _context.ModelArticle.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == articleId);
                return Result<ArticleModel>.Success(top);
            }
            catch(Exception ex)
            {
                return Result<ArticleModel>.Failure(new Error("GetArticleByIdAsync da xatolik", ex.Message));
            }
        }
    }
}
