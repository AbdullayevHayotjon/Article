using Article.Application.Services.IAuthServices;
using Article.Domain.Abstractions;
using Article.Domain.HelpModels.TempUserModel;
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

            User user = await _authRepository.IsUserExistsByEmailAsync(email);

            if (user is not null)
            {
                return Result<string>.Failure(UserError.CheckEmail); // Email allaqachon ro‘yxatdan o‘tgan
            }

            int code = await _authRepository.GenerateCode();

            try
            {
                TempUser OldTempUser = await _authRepository.IsTempUserExistsByEmailAsync(email);

                TempUser NewTempUser = new TempUser
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    VerificationCode = code,
                };

                if (OldTempUser is not null)
                {
                    await _authRepository.SaveUpdateVerificationCode(OldTempUser, NewTempUser);
                }
                else
                {
                    await _authRepository.SaveAddVerificationCode(NewTempUser);
                }

                await _unitOfWork.SaveChangesAsync();

                await _authRepository.SendVerificationEmail(email, code);

                return Result<string>.Success($"{email} ga tasdiqlash kodi yuborildi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email jo‘natishda xatolik: {ex.Message}");
                return Result<string>.Failure(new Error("Email jo‘natishda xatolik", ex.Message));
            }
        }

        public async Task<Result<string>> SignInService(SignInDTO signInDTO)
        {
            string email = signInDTO.Email.Trim().ToLower(); // Emailni tozalash

            User user = await _authRepository.IsUserExistsByEmailAsync(email);

            if (user is null)
            {
                return Result<string>.Failure(UserError.CheckEmail); // Email topilmadi
            }
            return Result<string>.Failure(UserError.CheckEmail);
        }
    }
}
