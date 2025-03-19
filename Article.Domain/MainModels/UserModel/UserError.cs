using Article.Domain.Abstractions;

namespace Article.Domain.MainModels.UserModel
{
    public class UserError
    {
        public static Error CheckEmail = new(
        "CheckEmail.Failed",
        "Bu email oldin ro'yhatdan o'tgan");
    }
}
