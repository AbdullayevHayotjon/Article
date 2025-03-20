using Article.Domain.HelpModels.TempUserModel;
using Article.Domain.MainModels.UserModel;

namespace Article.Domain.Models.UserModel.IAuthRepositories
{
    public interface IAuthRepository
    {
        Task<User> IsUserExistsByEmailAsync(string email);
        Task<int> GenerateCode();
        Task SaveAddVerificationCode(TempUser tempUser);
        Task<TempUser> IsTempUserExistsByEmailAsync(string email);
        Task SaveUpdateVerificationCode(TempUser oldTempUser, TempUser newTempUser);
        Task SendVerificationEmail(string email, int code);
        Task SendWelcomeEmail(string email, string fullName);
        Task<string> GenerateToken(string userId, string username, string role);
    }
}
