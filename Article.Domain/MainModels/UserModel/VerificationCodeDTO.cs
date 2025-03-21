using System.ComponentModel.DataAnnotations;

namespace Article.Domain.MainModels.UserModel
{
    public class VerificationCodeDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int Code { get; set; }
    }
}
