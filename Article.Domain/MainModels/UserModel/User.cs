using Article.Domain.Abstractions;
using Article.Domain.HelpModels.UserFollowingModel;
using Article.Domain.MainModels.ArticleModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.MainModels.UserModel
{
    [Table("Users", Schema = "MainSchema")]
    public class User : BaseParams
    {
        [Required(ErrorMessage = "FirstName bo‘sh bo‘lishi mumkin emas!")]
        [MinLength(2, ErrorMessage = "FirstName kamida 2 ta harf bo‘lishi kerak!")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName bo‘sh bo‘lishi mumkin emas!")]
        [MinLength(2, ErrorMessage = "LastName kamida 2 ta harf bo‘lishi kerak!")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email bo‘sh bo‘lishi mumkin emas!")]
        [EmailAddress(ErrorMessage = "Email formati noto‘g‘ri!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username bo‘sh bo‘lishi mumkin emas!")]
        [MinLength(3, ErrorMessage = "Username kamida 3 ta harfdan iborat bo‘lishi kerak!")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parol bo‘sh bo‘lishi mumkin emas!")]
        [MinLength(6, ErrorMessage = "Parol kamida 6 ta belgidan iborat bo‘lishi kerak!")]
        public string HashedPassword { get; set; } = string.Empty; // Hashed password saqlanadi

        [Required(ErrorMessage = "Role bo‘sh bo‘lishi mumkin emas!")]
        public UserRole Role { get; set; } = UserRole.User;

        public string AboutMe { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // Maqolalar bilan bog‘lash
        public virtual ICollection<ArticleModel> Articles { get; set; } = new List<ArticleModel>();

        // Following - foydalanuvchilar kuzatayotgan odamlar
        public virtual List<UserFollowing> Following { get; set; } = new List<UserFollowing>();

        // Followers - foydalanuvchini kuzatayotgan odamlar
        public virtual List<UserFollowing> Followers { get; set; } = new List<UserFollowing>();
    }

    public enum UserRole
    {
        User = 1,
        Technical = 2,
        Analyst = 3,
        Reviewer = 4,
        Admin = 5,
        SuperAdmin = 6
    }
}
