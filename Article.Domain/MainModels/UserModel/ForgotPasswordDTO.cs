using System.ComponentModel.DataAnnotations;

namespace Article.Domain.MainModels.UserModel
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
