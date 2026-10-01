using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SkillLoop.Application.Interfaces.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace SkillLoop.Infrasturcture.Services
{
    public class SocialLoginService : ISocialLoginService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        public SocialLoginService(HttpClient http, IConfiguration configuration) { _http = http; _configuration = configuration; }
        public async Task<SocialIdentity> ValidateAsync(string provider, string token, CancellationToken cancellationToken = default)
        {
            return provider.ToLowerInvariant() switch
            {
                "google" => await ValidateGoogleAsync(token, cancellationToken),
                "facebook" => await ValidateFacebookAsync(token, cancellationToken),
                "apple" => await ValidateAppleAsync(token, cancellationToken),
                _ => throw new InvalidOperationException("Unsupported social provider.")
            };
        }
        private async Task<SocialIdentity> ValidateGoogleAsync(string token, CancellationToken cancellationToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings { Audience = string.IsNullOrWhiteSpace(_configuration["SocialLogin:GoogleClientId"]) ? null : new[] { _configuration["SocialLogin:GoogleClientId"]! } });
            return new SocialIdentity("google", payload.Subject, payload.Email, payload.Name ?? payload.Email);
        }
        private async Task<SocialIdentity> ValidateFacebookAsync(string token, CancellationToken cancellationToken)
        {
            var url = $"https://graph.facebook.com/me?fields=id,name,email&access_token={Uri.EscapeDataString(token)}";
            var result = await _http.GetFromJsonAsync<FacebookUser>(url, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid Facebook token.");
            if (string.IsNullOrWhiteSpace(result.id) || string.IsNullOrWhiteSpace(result.email)) throw new UnauthorizedAccessException("Facebook account did not provide an email.");
            return new SocialIdentity("facebook", result.id, result.email, result.name ?? result.email);
        }
        private async Task<SocialIdentity> ValidateAppleAsync(string token, CancellationToken cancellationToken)
        {
            var manager = new ConfigurationManager<OpenIdConnectConfiguration>("https://appleid.apple.com/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever { RequireHttps = true });
            var configuration = await manager.GetConfigurationAsync(cancellationToken);
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters { ValidateIssuer = true, ValidIssuer = "https://appleid.apple.com", ValidateAudience = true, ValidAudience = _configuration["SocialLogin:AppleClientId"], ValidateLifetime = true, ValidateIssuerSigningKey = true, IssuerSigningKeys = configuration.SigningKeys };
            var principal = handler.ValidateToken(token, parameters, out _);
            var email = principal.FindFirst("email")?.Value;
            var subject = principal.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(email)) throw new UnauthorizedAccessException("Invalid Apple identity token.");
            return new SocialIdentity("apple", subject, email, email);
        }
        private sealed record FacebookUser(string id, string? name, string? email);
    }
}
