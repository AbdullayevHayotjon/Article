
//using Article.Application.Services.IArticleServices;
//using Article.Domain.MainModels.ArticleModels;
//using Article.Domain.MainModels.UserModel;
//using Microsoft.AspNetCore.Http;
//using System;

//namespace Article.Application.Services.ArticleServices
//{
//    public class ArticleService : IArticleService
//    {
//        private readonly AppDbContext _context;

//        public ArticleService(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ArticleModel> UploadArticleAsync(IFormFile file, string title, string authorName)
//        {
//            if (file == null || file.Length == 0)
//                throw new ArgumentException("Fayl yuklanmadi.");

//            if (Path.GetExtension(file.FileName).ToLower() != ".docx")
//                throw new ArgumentException("Faqat .docx formatdagi fayllarni yuklash mumkin.");

//            using (var memoryStream = new MemoryStream())
//            {
//                await file.CopyToAsync(memoryStream);
//                var fileData = memoryStream.ToArray(); // Faylni byte[] formatga o‘tkazamiz

//                var article = new ArticleModel
//                {
//                    Title = title,
                    
//                    // = authorName,
//                    FileData = fileData,  // Bazaga saqlaymiz
//                    ContentType = file.ContentType,
//                    Status = ArticleStatus.Pending
//                };

//                _context.Articles.Add(article);
//                await _context.SaveChangesAsync();

//                return article;
//            }
//        }

//        public async Task<byte[]> DownloadArticleAsync(Guid articleId)
//        {
//            var article = await _context.Articles.FindAsync(articleId);
//            if (article == null || article.FileData == null)
//                throw new FileNotFoundException("Fayl topilmadi.");

//            return article.FileData; // Bazadan faylni qaytarish
//        }

//        public async Task<ArticleModel> ResubmitArticleAsync(Guid articleId, IFormFile file)
//        {
//            var article = await _context.Articles.FindAsync(articleId);
//            if (article == null)
//                throw new ArgumentException("Maqola topilmadi.");

//            if (article.Status != ArticleStatus.Rejected)
//                throw new ArgumentException("Faqat rad etilgan maqolalar qayta yuklanishi mumkin.");

//            if (file == null || file.Length == 0)
//                throw new ArgumentException("Fayl yuklanmadi.");

//            if (Path.GetExtension(file.FileName).ToLower() != ".docx")
//                throw new ArgumentException("Faqat .docx formatdagi fayllarni yuklash mumkin.");

//            using (var memoryStream = new MemoryStream())
//            {
//                await file.CopyToAsync(memoryStream);
//                article.FileData = memoryStream.ToArray(); // Faylni yangilash
//                article.ContentType = file.ContentType;
//                article.Status = ArticleStatus.Pending;
//                article.ReviewedAt = null;

//                await _context.SaveChangesAsync();
//                return article;
//            }
//        }

//        public async Task<Article?> GetArticleByIdAsync(Guid articleId)
//        {
//            return await _context.Articles.FindAsync(articleId);
//        }
//    }
//}
