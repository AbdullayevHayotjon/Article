
namespace Article.Domain.MainModels.UserModel
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public string Category { get; set; }
    }
}
