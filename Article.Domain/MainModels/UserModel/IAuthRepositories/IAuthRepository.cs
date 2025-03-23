using Article.Domain.HelpModels.PasswordResetModel;
using Article.Domain.HelpModels.RefreshTokenModel;
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
        Task SendMessageEmail(string toEmail, string subject, string body);
        Task<string> GenerateAccessToken(Guid userId, string username, UserRole role);
        Task<string> GenerateRefreshToken(Guid userId);
        Task AddAsync(User user);
        Task<string> GenerateUniqueUsernameAsync();
        Task<bool> IsUsernameExistsAsync(string username);
        Task TempUserDelete(TempUser tempUser);
        Task<RefreshToken> GetToken(string refreshToken);
        Task<User> GetByIdAsync(Guid id);
        Task ExistingReset(PasswordReset passwordReset);
        Task<PasswordReset> PasswordReset(string token);
        Task RemovePasswordReset(PasswordReset passwordReset);
    }
}
