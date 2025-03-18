using Article.Application.Data;
using Article.Application.Services;
using Article.Application.Services.IArticleServices;
using Article.Application.Services.IAuthServices;
using Article.Domain.Abstractions;
using Article.Domain.Models.UserModel.IAuthRepositories;
using Article.Infrastructure.ArticleServices;
using Article.Infrastructure.Data;
using Article.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Article.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureRegisterServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
              configuration.GetConnectionString("DefaultConnection") ??
              throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                object value = options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
            });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped(typeof(Repository<>));
           
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnection>(_ => new SqlConnection(connectionString));

            return services;
        }
    }
}
