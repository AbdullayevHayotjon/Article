using Article.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.PasswordResetModel
{
    [Table("PasswordResets", Schema = "HelpSchema")]
    public class PasswordReset : Entity
    {
        [Required]
        public Guid UserId { get; set; } // Foydalanuvchi ID si
        [Required]
        public string Token { get; set; } = string.Empty;// Parolni tiklash tokeni
        [Required]
        public DateTime ExpiryDate { get; set; } // Amal qilish muddati
    }

}
