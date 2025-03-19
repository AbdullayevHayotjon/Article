using System.ComponentModel.DataAnnotations;

namespace Article.Domain.MainModels.UserModel
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Ism majburiy!")]
        [MinLength(2, ErrorMessage = "Ism kamida 2 ta harf bo‘lishi kerak!")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Familiya majburiy!")]
        [MinLength(2, ErrorMessage = "Familiya kamida 2 ta harf bo‘lishi kerak!")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email majburiy!")]
        [EmailAddress(ErrorMessage = "Email noto‘g‘ri formatda!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parol majburiy!")]
        [MinLength(6, ErrorMessage = "Parol kamida 6 ta belgidan iborat bo‘lishi kerak!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parolni tasdiqlash majburiy!")]
        [Compare("Password", ErrorMessage = "Parollar mos kelmadi!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
