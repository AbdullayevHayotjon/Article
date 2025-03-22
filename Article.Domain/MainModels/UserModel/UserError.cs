using Article.Domain.Abstractions;

namespace Article.Domain.MainModels.UserModel
{
    public class UserError
    {
        public static Error CheckRegisterEmail = new(
        "CheckRegisterEmail.Failed",
        "Bu email oldin ro'yhatdan o'tgan");
        public static Error CheckSignInEmail = new(
        "CheckSignInEmail.Failed",
        "Hali ro'yhatdan o'tmagansiz, iltimos Register qiling");
        public static Error CheckSignInPassword = new(
        "CheckSignInPessword.Failed",
        "Parol xato!");
        public static Error CheckVerificationEmail = new(
        "CheckVerificationEmail.Failed",
        "Email topilmadi!");
        public static Error CheckVerificationCode = new(
        "CheckVerificationCode.Failed",
        "Tasdiqlash kodi noto‘g‘ri");
        public static Error CheckVerificationCodeDate = new(
        "CheckVerificationCodeDate.Failed",
        "Tasdiqlash kodi muddati o‘tgan");
        public static Error CheckRefreshToken = new(
        "Refresh token noto‘g‘ri",
        "Berilgan refresh token bazada topilmadi.");
        public static Error CheckRefreshTokenDate = new(
        "Refresh token eskirgan",
        "Yangi login qilish talab etiladi.");
        public static Error CheckUser = new(
        "Foydalanuvchi topilmadi",
        "Berilgan refresh tokenga mos foydalanuvchi yo‘q.");
        public static Error checkFileUpload = new("Fayl yuklanmadi", "Fayl yuklanmadi.");
        public static Error ErrodFormatFile = new("it isn't .doc", "Faqat .docx formatdagi fayllarni yuklash mumkin.");
        public static Error ErrorUploadArticleAsync = new("UploadArticleAsync", "xatolik articleServiseda");
        public static Error ErrorArticleDontFind = new("Maqola topilmadi.", "maqola avval qaytarilmagan yoki mavjud emas");
        public static Error rejectedError = new("maqola rad etilmagan", "Faqat rad etilgan maqolalar qayta yuklanishi mumkin.");
    }
}
