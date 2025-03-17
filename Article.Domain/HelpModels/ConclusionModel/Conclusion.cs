using Article.Domain.Abstractions;
using Article.Domain.MainModels.ArticleModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.ConclusionModel
{
    [Table("Conclusions", Schema = "HelpSchema")]
    public class Conclusion : BaseParams
    {
        public string Summary { get; set; } = string.Empty;

        // Maqolaga bog‘lash uchun Foreign Key
        public Guid ArticleId { get; set; }
        public ArticleModel Article { get; set; } = null!;
    }
}
