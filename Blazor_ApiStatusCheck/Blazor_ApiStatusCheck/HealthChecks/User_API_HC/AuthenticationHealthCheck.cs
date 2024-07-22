using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Reflection.Metadata;
using System.Text;

namespace Blazor_ApiStatusCheck.HealthChecks.User_API_HC
{
    public class AuthenticationHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly string _authUrl;
        private readonly string _username;
        private readonly string _password;

        public AuthenticationHealthCheck(HttpClient httpClient,string authUrl,string username,string password)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authUrl = authUrl ?? throw new ArgumentNullException(nameof(_authUrl));
            _username = username ?? throw new ArgumentNullException(nameof(_username));
            _password = password ?? throw new ArgumentNullException(nameof(_password));
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
          
            
          
            var request = new HttpRequestMessage(HttpMethod.Post, _authUrl);
            var credentials = new {username = _username, password = _password};
            request.Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(credentials),
                encoding: Encoding.UTF8,
                "application/json"
                );
            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("Authentication is working.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("Authentication failed");
                }
            }
            catch (Exception ex) { 
                return HealthCheckResult.Unhealthy($"Authentication check failed: {ex.Message}");
            }
        }
    }
}
