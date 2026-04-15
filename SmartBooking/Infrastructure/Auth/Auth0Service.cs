using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace SmartBooking.Infrastructure.Auth
{
    public class Auth0Service
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public Auth0Service(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<string> ObtenerToken()
        {
            var domain = _config["Auth0M2M:Domain"];
            var clientId = _config["Auth0M2M:ClientId"];
            var clientSecret = _config["Auth0M2M:ClientSecret"];

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://{domain}/oauth/token"
            );

            var body = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                audience = $"https://{domain}/api/v2/",
                grant_type = "client_credentials"
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var data = JsonDocument.Parse(content).RootElement;

            return data.GetProperty("access_token").GetString();
        }

        public async Task GuardarMetadata(string userId, object metadata)
        {
            var domain = _config["Auth0:Domain"];
            var token = await ObtenerToken();

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                user_metadata = metadata
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            await _http.PatchAsync(
                $"https://{domain}/api/v2/users/{userId}",
                content
            );
        }
    }
}