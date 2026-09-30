using Microsoft.Extensions.Configuration;
using SkillLoop.Application.Interfaces.IService;
using System.Net.Http.Json;

namespace SkillLoop.Infrasturcture.Services
{
    public class HttpSmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpSmsService(HttpClient httpClient, IConfiguration configuration) { _httpClient = httpClient; _configuration = configuration; }
        public async Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            var url = _configuration["Sms:Url"];
            var apiKey = _configuration["Sms:ApiKey"];
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(apiKey)) throw new InvalidOperationException("SMS provider is not configured.");
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-Api-Key", apiKey);
            request.Content = JsonContent.Create(new { to = phoneNumber, message });
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
