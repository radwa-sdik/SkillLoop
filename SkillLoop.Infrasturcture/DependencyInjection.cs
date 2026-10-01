using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillLoop.Application.Interfaces.IService;
using SkillLoop.Infrasturcture.Data;
using SkillLoop.Infrasturcture.Security;
using SkillLoop.Infrasturcture.Services;

namespace SkillLoop.Infrasturcture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.Configure<EmailOptions>(configuration.GetSection("Email"));
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddHttpClient<ISocialLoginService, SocialLoginService>();
            return services;
        }
    }
}