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
        Task<string> GenerateAccessToken(Guid userId, string username, UserRole role);
        Task<string> GenerateRefreshToken(Guid userId);
        Task AddAsync(User user);
        Task SendWelcomeEmail(string email, string fullName);
        Task<string> GenerateUniqueUsernameAsync();
        Task<bool> IsUsernameExistsAsync(string username);
        Task TempUserDelete(TempUser tempUser);
    }
}
