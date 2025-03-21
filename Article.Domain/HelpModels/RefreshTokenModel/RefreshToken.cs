using Article.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.RefreshTokenModel
{
    [Table("RefreshTokens", Schema = "HelpSchema")]
    public class RefreshToken : Entity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
