using Article.Domain.MainModels.UserModel;
using Article.Domain.Models.UserModel.IAuthRepositories;

namespace Article.Infrastructure.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        public AuthRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
