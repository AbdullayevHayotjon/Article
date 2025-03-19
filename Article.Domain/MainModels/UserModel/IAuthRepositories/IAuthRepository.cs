namespace Article.Domain.Models.UserModel.IAuthRepositories
{
    public interface IAuthRepository
    {
        Task<bool> IsUserExistsByEmailAsync(string email);
        Task<int> GenerateCode();
        Task SendWelcomeEmail(string email, string fullName);
        Task SendVerificationEmail(string email, int code);
        Task<string> GenerateToken(string userId, string username, string role);
    }
}
