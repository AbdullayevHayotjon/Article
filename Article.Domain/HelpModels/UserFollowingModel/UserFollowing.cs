using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.UserFollowingModel
{
    [Table("UserFollowings", Schema = "HelpSchema")]
    public class UserFollowing : BaseParams
    {
        public Guid FollowerId { get; set; }
        public virtual User? Follower { get; set; }

        public Guid FollowingId { get; set; }
        public virtual User? Following { get; set; }
    }
}
