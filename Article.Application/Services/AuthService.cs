using Article.Application.Services.IAuthServices;
using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;
using Article.Domain.Models.UserModel.IAuthRepositories;

namespace Article.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthRepository _authRepository;
        public AuthService(IUnitOfWork unitOfWork, IAuthRepository authRepository)
        {
            _unitOfWork = unitOfWork;
            _authRepository = authRepository;
        }

        public async Task<Result<string>> SignUpService(RegisterDTO model)
        {
            string email = model.Email.Trim().ToLower(); // Emailni tozalash

            if (await _authRepository.IsUserExistsByEmailAsync(email))
            {
                return Result<string>.Failure(UserError.CheckEmail); // Email allaqachon ro‘yxatdan o‘tgan
            }

            int code = await _authRepository.GenerateCode();

            try
            {
                await _authRepository.SendVerificationEmail(email, code);

                // Tasdiqlash kodini vaqtinchalik saqlash (agar kerak bo‘lsa)
                // await _authRepository.SaveVerificationCode(email, code);

                return Result<string>.Success($"{email} ga tasdiqlash kodi yuborildi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email jo‘natishda xatolik: {ex.Message}");
                return Result<string>.Failure(new Error("Email jo‘natishda xatolik", ex.Message));
            }
        }

    }
}
