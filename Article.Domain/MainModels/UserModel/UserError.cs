using Article.Domain.Abstractions;

namespace Article.Domain.MainModels.UserModel
{
    public class UserError
    {
        public static Error CheckEmail = new(
        "CheckEmail.Failed",
        "Bu email oldin ro'yhatdan o'tgan");
        public static Error checkFileUpload = new("Fayl yuklanmadi", "Fayl yuklanmadi.");
        public static Error ErrodFormatFile = new("it isn't .doc", "Faqat .docx formatdagi fayllarni yuklash mumkin.");
        public static Error ErrorUploadArticleAsync = new("UploadArticleAsync", "xatolik articleServiseda");
        public static Error ErrorArticleDontFind = new("Maqola topilmadi.", "maqola avval qaytarilmagan yoki mavjud emas");
        public static Error rejectedError = new("maqola rad etilmagan", "Faqat rad etilgan maqolalar qayta yuklanishi mumkin.");
    }
}
