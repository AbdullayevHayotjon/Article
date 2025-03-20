using Article.Application.Data;
using Article.Domain.MainModels.UserModel;
using Article.Domain.Models.UserModel.IAuthRepositories;
using Article.Domain.StaticModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using MimeKit;
using MailKit.Security;
using Article.Domain.HelpModels.TempUserModel;

namespace Article.Infrastructure.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly ISqlConnection _sqlConnection;
        private readonly SmtpSettings _smtpSettings;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;
        public AuthRepository(ApplicationDbContext context, IOptions<SmtpSettings> smtpSettings,
        ApplicationDbContext applicationDbContext,
        ISqlConnection sqlConnection, IConfiguration configuration) : base(context)
        {
            _smtpSettings = smtpSettings.Value;
            _applicationDbContext = applicationDbContext;
            _sqlConnection = sqlConnection;
            _configuration = configuration;
        }

        public async Task<User> IsUserExistsByEmailAsync(string email)
        {
                return await _applicationDbContext.Users
                    .FirstOrDefaultAsync(user => user.Email == email && !user.IsDeleted);
        }


        public async Task<int> GenerateCode()
        {
            return RandomNumberGenerator.GetInt32(1000, 10000);
        }

        public async Task SaveAddVerificationCode(TempUser tempUser)
        {
            await _applicationDbContext.TempUsers.AddAsync(tempUser);
        }

        public async Task<TempUser> IsTempUserExistsByEmailAsync(string email)
        {
            return await _applicationDbContext.TempUsers
                .FirstOrDefaultAsync(user => user.Email == email && !user.IsDeleted);
        }

        public async Task SaveUpdateVerificationCode(TempUser oldTempUser, TempUser newTempUser)
        {
            oldTempUser.Firstname = newTempUser.Firstname;
            oldTempUser.Lastname = newTempUser.Lastname;
            oldTempUser.Email = newTempUser.Email;
            oldTempUser.HashedPassword = newTempUser.HashedPassword;
            oldTempUser.VerificationCode = newTempUser.VerificationCode;
            oldTempUser.UpdateDate = DateTime.UtcNow;
        }
        public async Task SendVerificationEmail(string email, int code)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(email, email));
                message.Subject = "Tasdiqlash kodi";

                // To‘g‘ri yo‘lni aniqlash
                string baseDirectory = AppContext.BaseDirectory;

                // Loyiha ildiz katalogini olish
                string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.FullName;

                // To‘g‘ri yo‘lni hosil qilish
                string templatePath = Path.Combine(projectRoot, "Article.Infrastructure", "Templates", "verification_email.html");

                string body = File.ReadAllText(templatePath).Replace("{{CODE}}", code.ToString());

                message.Body = new TextPart("html") { Text = body };

                using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtpClient.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email jo‘natishda xatolik: {ex.Message}");
            }
        }

        public async Task SendWelcomeEmail(string email, string fullName)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = "Xush kelibsiz",
                Body = EmailTemplate.GetWelcomeEmailTemplate(email, fullName),
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }

        
        public async Task<string> GenerateToken(string userId, string username, string role)
        {
            // Konfiguratsiya sozlamalarini olish
            var secretKey = _configuration["JwtSettings:Secret"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = int.TryParse(_configuration["JwtSettings:ExpiryMinutes"], out var result) ? result : 60; // Default to 60 minutes

            // Tokenni shifrlash uchun kalit
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token ichidagi foydalanuvchi ma'lumotlari (Claims)
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),  // User ID
        new Claim(JwtRegisteredClaimNames.UniqueName, username),    // Username
        new Claim(ClaimTypes.Role, role),                           // Role
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique Token ID
    };

            // Tokenni yaratish
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
