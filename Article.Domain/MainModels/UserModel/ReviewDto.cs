namespace Article.Domain.MainModels.UserModel
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ReviewerId { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewedAt { get; set; }
    }
}
