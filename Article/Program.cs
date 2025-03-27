using Article.Domain.StaticModels;
using Article.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Article
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // SmtpSettings sozlamalarini yuklash
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

            // JWT sozlamalarini yuklash
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? throw new ArgumentNullException("JWT Secret not found!");

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            // JWT autentifikatsiya tizimini sozlash
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // HTTPS talab qilinmaydi
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            // CORS sozlamalari - Hamma kelayotgan so'rovlarni ruxsat berish
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });



            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5093); // Hamma IP-larni tinglash
            });

            builder.WebHost.UseUrls("http://0.0.0.0:5093");

            // Servislarni qo‘shish
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Bearer tokenni kiriting"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Ma'lumotlar bazasi va boshqa servislar
            builder.Services.AddInfrastructureRegisterServices(builder.Configuration);

            var app = builder.Build();

            // CORS-ni qo'shish
            app.UseCors("AllowAll");

            // Xatoliklarni kuzatish uchun middleware
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("SwaggerEnable"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // HTTPS ni o‘chirib qo‘yamiz, chunki Swagger HTTP orqali ishlayapti
            // app.UseHttpsRedirection();

            // JWT autentifikatsiyasini qo‘shish
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}