using Article.Domain.MainModels.UserModel;

namespace Article.Domain.MainModels.ArticleModels
{
    public class ArticleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public string FileUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public ArticleStatus Status { get; set; }
        public UserDTO User { get; set; }
    }
}
