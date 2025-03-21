using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;

namespace Article.Application.Services.IAuthServices
{
    public interface IAuthService
    {
        Task<Result<string>> SignUpService(RegisterDTO model);
        Task<Result<TokenResponse>> SignInService(SignInDTO signInDTO);
        Task<Result<string>> VerificationCodeService(VerificationCodeDTO verificationCodeDTO);
    }
}
