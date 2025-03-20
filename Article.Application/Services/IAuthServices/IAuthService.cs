using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;

namespace Article.Application.Services.IAuthServices
{
    public interface IAuthService
    {
        Task<Result<string>> SignUpService(RegisterDTO model);
        Task<Result<string>> SignInService(SignInDTO signInDTO);
    }
}
