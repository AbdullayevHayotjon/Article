using Article.Domain.Abstractions;
using Article.Domain.HelpModels.ConclusionModel;
using Article.Domain.MainModels.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.MainModels.ArticleModels
{
    [Table("Articles", Schema = "MainSchema")]
    public class ArticleModel : BaseParams
    {
        [Required(ErrorMessage = "Maqola sarlavhasi bo‘sh bo‘lishi mumkin emas!")]
        [MinLength(5, ErrorMessage = "Sarlavha kamida 5 ta belgidan iborat bo‘lishi kerak!")]
        [MaxLength(100, ErrorMessage = "Sarlavha maksimal 100 ta belgidan oshmasligi kerak!")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategoriya bo‘sh bo‘lishi mumkin emas!")]
        public string Category { get; set; } = string.Empty;

        public int ViewCount { get; set; } = 0;
        public int DownloadCount { get; set; } = 0;

        [Required(ErrorMessage = "Maqolaning yuklanish fayli bo‘sh bo‘lishi mumkin emas!")]
        public string FileUrl { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Maqola holati bo‘sh bo‘lishi mumkin emas!")]
        public ArticleStatus Status { get; set; } = ArticleStatus.Pending;

        // Maqola xulosasi bilan bog‘lash
        public Conclusion? Conclusion { get; set; }

        // User bilan bog‘lash
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }

    public enum ArticleStatus
    {
        Pending = 1,  // Ko‘rib chiqish jarayonida
        Approved = 2, // Tasdiqlangan
        Rejected = 3  // Rad etilgan
    }
}
