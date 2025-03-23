using System.ComponentModel.DataAnnotations;

namespace Article.Domain.MainModels.UserModel
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Parol kamida 6 ta belgidan iborat bo‘lishi kerak!")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Parollar mos kelmadi!")]
        public string NewPasswordConfirm { get; set; }
    }
}
