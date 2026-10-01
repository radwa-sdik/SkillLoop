using Microsoft.Extensions.Options;
using SkillLoop.Application.Interfaces.IService;
using System.Net.Http.Headers;
using System.Text;

namespace SkillLoop.Infrasturcture.Services
{
    public class SmsOptions
    {
        public string BaseUrl { get; set; } = "https://api.twilio.com";
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string FromNumber { get; set; } = string.Empty;
    }

    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly SmsOptions _options;

        public SmsService(HttpClient httpClient, IOptions<SmsOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.AccountSid) ||
                string.IsNullOrWhiteSpace(_options.AuthToken) ||
                string.IsNullOrWhiteSpace(_options.FromNumber))
            {
                throw new InvalidOperationException("SMS provider is not configured.");
            }

            var endpoint = $"{_options.BaseUrl.TrimEnd('/')}/2010-04-01/Accounts/{_options.AccountSid}/Messages.json";

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.AccountSid}:{_options.AuthToken}"));

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["To"] = phoneNumber,
                ["From"] = _options.FromNumber,
                ["Body"] = message
            });

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to send SMS.");
        }
    }
}