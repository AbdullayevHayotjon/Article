using Article.Application.Services.IAuthServices;
using Article.Domain.Abstractions;
using Article.Domain.HelpModels.PasswordResetModel;
using Article.Domain.HelpModels.RefreshTokenModel;
using Article.Domain.HelpModels.TempUserModel;
using Article.Domain.MainModels.UserModel;
using Article.Domain.Models.UserModel.IAuthRepositories;
using System.Security.Cryptography;

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
                return Result<string>.Failure(UserError.CheckRegisterEmail); // Email allaqachon ro‘yxatdan o‘tgan
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

                string baseDirectory = AppContext.BaseDirectory;
                string projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
                string templatePath = Path.Combine(projectRoot, "Article.Infrastructure", "Templates", "verification_email.html");
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{CODE}}", code.ToString());
                await _authRepository.SendMessageEmail(email, "Tasdiqlash kodi", body);

                return Result<string>.Success($"{email} ga tasdiqlash kodi yuborildi");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error("Email jo‘natishda xatolik", ex.Message));
            }
        }

        public async Task<Result<TokenResponse>> SignInService(SignInDTO signInDTO)
        {
            try
            {
                string email = signInDTO.Email.Trim().ToLower(); // Emailni tozalash

                User user = await _authRepository.IsUserExistsByEmailAsync(email);

                if (user is null)
                    return Result<TokenResponse>.Failure(UserError.CheckSignInEmail); // ❌ Email topilmadi

                if (!BCrypt.Net.BCrypt.Verify(signInDTO.Password, user.HashedPassword))
                    return Result<TokenResponse>.Failure(UserError.CheckSignInPassword); // ❌ Parol noto‘g‘ri

                string accessToken = await _authRepository.GenerateAccessToken(user.Id, user.Username, user.Role);
                string refreshToken = await _authRepository.GenerateRefreshToken(user.Id);

                await _unitOfWork.SaveChangesAsync(); // 🔄 Refresh tokenni saqlash

                return Result<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return Result<TokenResponse>.Failure(new Error("SignIn da xatolik", ex.Message));
            }
        }

        public async Task<Result<string>> VerificationCodeService(VerificationCodeDTO verificationCodeDTO)
        {
            try
            {
                string email = verificationCodeDTO.Email.Trim().ToLower(); // Emailni tozalash

                TempUser tempUser = await _authRepository.IsTempUserExistsByEmailAsync(email);

                if (tempUser is null)
                    return Result<string>.Failure(UserError.CheckVerificationEmail); // ❌ Email topilmadi

                if (tempUser.VerificationCode != verificationCodeDTO.Code)
                {
                    return Result<string>.Failure(UserError.CheckVerificationCode); // ❌ Tasdiqlash kodi noto‘g‘ri
                }

                if (DateTime.UtcNow > tempUser.ExpirationTime)
                {
                    return Result<string>.Failure(UserError.CheckVerificationCode); // ❌ Tasdiqlash kodi muddati o‘tgan
                }

                string username;
                do
                {
                    username = await _authRepository.GenerateUniqueUsernameAsync(); // Tasodifiy username yaratish
                }
                while (await _authRepository.IsUsernameExistsAsync(username)); // Bazada bor-yo‘qligini tekshirish

                User user = new User
                {
                    Firstname = tempUser.Firstname,
                    Lastname = tempUser.Lastname,
                    Email = tempUser.Email,
                    HashedPassword = tempUser.HashedPassword,
                    Username = username
                };

                await _authRepository.AddAsync(user);

                await _authRepository.TempUserDelete(tempUser);

                await _unitOfWork.SaveChangesAsync();

                string baseDirectory = AppContext.BaseDirectory;
                string projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
                string templatePath = Path.Combine(projectRoot, "Article.Infrastructure", "Templates", "welcome_user.html");
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{FULLNAME}}", $"{tempUser.Firstname} {tempUser.Lastname}");
                await _authRepository.SendMessageEmail(email, "Xush kelibsiz!", body);

                return Result<string>.Success($"{email} muvaffaqiyatli ro‘yxatdan o‘tdi");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error("VerificationCode da xatolik", ex.Message));
            }
        }

        public async Task<Result<TokenResponse>> RefreshTokenService(RefreshTokenDTO refreshTokenDto)
        {
            try
            {
                // 1. Bazadan refresh tokenni topish
                RefreshToken refreshTokenEntity = await _authRepository.GetToken(refreshTokenDto.RefreshToken);

                if (refreshTokenEntity == null)
                {
                    return Result<TokenResponse>.Failure(UserError.CheckRefreshToken);
                }

                // 2. Tokenning yaroqlilik muddatini tekshirish
                if (refreshTokenEntity.ExpiryDate < DateTime.UtcNow)
                {
                    return Result<TokenResponse>.Failure(UserError.CheckRefreshTokenDate);
                }

                // 3. Foydalanuvchi ma’lumotlarini olish
                var user = await _authRepository.GetByIdAsync(refreshTokenEntity.UserId);
                if (user == null)
                {
                    return Result<TokenResponse>.Failure(UserError.CheckUserByRefreshToken);
                }

                // 4. Yangi tokenlar yaratish
                var newAccessToken = await _authRepository.GenerateAccessToken(user.Id, user.Username, user.Role);
                var newRefreshToken = await _authRepository.GenerateRefreshToken(user.Id);

                await _unitOfWork.SaveChangesAsync();

                // 5. Yangi tokenlarni qaytarish    
                return Result<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return Result<TokenResponse>.Failure(new Error("Refresh tokenda xatolik", ex.Message));
            }
        }

        public async Task<Result<string>> ForgotPasswordService(ForgotPasswordDTO forgotPassword)
        {
            try
            {
                string email = forgotPassword.Email.Trim().ToLower(); // Emailni tozalash

                User user = await _authRepository.IsUserExistsByEmailAsync(email);

                if (user is null)
                {
                    return Result<string>.Failure(UserError.CheckUserByEmail);
                }

                // 1. Reset token yaratish
                string resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                var passwordReset = new PasswordReset
                {
                    UserId = user.Id,
                    Token = resetToken,
                    ExpiryDate = DateTime.UtcNow.AddHours(1) // 1 soat amal qiladi
                };

                // 2. Eski tokenni o‘chirish va yangisini qo‘shish
                await _authRepository.ExistingReset(passwordReset);

                await _unitOfWork.SaveChangesAsync();

                // 3. Email jo‘natish
                string resetLink = $"https://yourwebsite.com/reset-password?token={resetToken}";
                string baseDirectory = AppContext.BaseDirectory;
                string projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
                string templatePath = Path.Combine(projectRoot, "Article.Infrastructure", "Templates", "reset_password.html");
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{RESET_LINK}}", resetLink);
                await _authRepository.SendMessageEmail(email, "Parolni tiklash", body);

                return Result<string>.Success($"Parolni tiklash havolasi {email} ga yuborildi.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error("Forgot Passwordda xatolik", ex.Message));
            }
        }

        public async Task<Result<string>> ResetPasswordService(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                // Tokenni tekshirish
                var passwordReset = await _authRepository.PasswordReset(resetPasswordDto.Token);

                if (passwordReset is null)
                {
                    return Result<string>.Failure(new Error("Reset Token noto‘g‘ri", "Token bazada topilmadi yoki noto‘g‘ri."));
                }

                // Tokenning amal qilish muddatini tekshirish
                if (passwordReset.ExpiryDate < DateTime.UtcNow)
                {
                    return Result<string>.Failure(new Error("Reset Token muddati o‘tgan", "Tokenning amal qilish vaqti tugagan."));
                }

                // Foydalanuvchini topish
                var user = await _authRepository.GetByIdAsync(passwordReset.UserId);

                if (user is null)
                {
                    return Result<string>.Failure(UserError.CheckUserById);
                }

                // Yangi parolni hash qilish
                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                user.UpdateDate = DateTime.UtcNow;

                // Parolni yangilash va tokenni o‘chirish
                await _authRepository.RemovePasswordReset(passwordReset);

                await _unitOfWork.SaveChangesAsync();

                string baseDirectory = AppContext.BaseDirectory;
                string projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
                string templatePath = Path.Combine(projectRoot, "Article.Infrastructure", "Templates", "welcome_reset_password.html");
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{FULLNAME}}", $"{user.Firstname} {user.Lastname}");
                await _authRepository.SendMessageEmail(user.Email, "Muvaffaqqiyatli", body);

                return Result<string>.Success("Parolingiz muvaffaqiyatli yangilandi. Endi tizimga yangi parol bilan kiring.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error("Reset Passwordda xatolik", ex.Message));
            }
        }
        
        public async Task<Result<string>> LogoutService(Guid userId)
        {
            try
            {
                // Foydalanuvchining refresh tokenini bazadan o‘chirish
                await _authRepository.RemoveRefreshTokenByUserId(userId);

                return Result<string>.Success("Tizimdan muvaffaqqiyatli chiqildi.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error("Logoutda xatolik", ex.Message));
            }
        }

    }
}
