using Article.Domain.Abstractions;
using Article.Domain.HelpModels.CategoryModel;
using Article.Domain.HelpModels.ConclusionModel;
using Article.Domain.HelpModels.PasswordResetModel;
using Article.Domain.HelpModels.RefreshTokenModel;
using Article.Domain.HelpModels.ReviewModel;
using Article.Domain.HelpModels.Specialization_Model;
using Article.Domain.HelpModels.TempUserModel;
using Article.Domain.HelpModels.UserFollowingModel;
using Article.Domain.MainModels.ArticleModels;
using Article.Domain.MainModels.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ArticleModel> ModelArticle { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<Conclusion> Conclusions { get; set; }
        public DbSet<TempUser> TempUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Category> Categorys { get; set; }

    }
}
