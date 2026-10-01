using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SkillLoop.API.Filters;
using SkillLoop.Application;
using SkillLoop.Infrasturcture;
using System.Text;

namespace SkillLoop.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationFilter>();
            });
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);

            var jwt = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is required."))),
                        ValidateIssuer = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwt["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}