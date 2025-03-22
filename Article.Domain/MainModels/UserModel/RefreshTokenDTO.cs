using System.ComponentModel.DataAnnotations;

namespace Article.Domain.MainModels.UserModel
{
    public class RefreshTokenDTO
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
