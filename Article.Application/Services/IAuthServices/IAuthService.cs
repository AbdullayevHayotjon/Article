using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;

namespace Article.Application.Services.IAuthServices
{
    public interface IAuthService
    {
        Task<Result<string>> SignUpService(RegisterDTO model);
        Task<Result<TokenResponse>> SignInService(SignInDTO signInDTO);
        Task<Result<string>> VerificationCodeService(VerificationCodeDTO verificationCodeDTO);
        Task<Result<TokenResponse>> RefreshTokenService(RefreshTokenDTO refreshToken);
        Task<Result<string>> ForgotPasswordService(ForgotPasswordDTO forgotPassword);
        Task<Result<string>> ResetPasswordService(ResetPasswordDTO resetPasswordDto);
    }
}
