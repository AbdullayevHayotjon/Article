
using Article.Domain.MainModels.UserModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Article.Domain.MainModels.ArticleModels;

namespace Article.Domain.HelpModels.ReviewModel
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; } // Taqrizning noyob identifikatori

        [ForeignKey("Article")]
        public Guid ArticleId { get; set; } // Qaysi maqolaga tegishli ekanligi
        public ArticleModel Article { get; set; }

        [ForeignKey("Reviewer")]
        public Guid ReviewerId { get; set; } // Taqrizni yozgan shaxs
        public User Reviewer { get; set; }

        [Required]
        public string Comments { get; set; } // Taqriz matni (izoh)

        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow; // Taqriz sanasi
    }
}
