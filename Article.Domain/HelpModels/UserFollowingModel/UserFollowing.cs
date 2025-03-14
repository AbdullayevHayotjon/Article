using Article.Domain.Abstractions;
using Article.Domain.MainModels.UserModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Article.Domain.HelpModels.UserFollowingModel
{
    [Table("UserFollowings", Schema = "HelpSchema")]
    public class UserFollowing : BaseParams
    {
        public int FollowerId { get; set; }
        public virtual User? Follower { get; set; }

        public int FollowingId { get; set; }
        public virtual User? Following { get; set; }
    }
}
