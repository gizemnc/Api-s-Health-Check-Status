using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_ApiStatusCheck.HealthChecks
{
    public class InventoryHealthCheck : IHealthCheck
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public InventoryHealthCheck(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var apiUrl = _configuration["ApiSettings:InventoryApi:BaseUrl"];
            var healthEndpoint = _configuration["ApiSettings:InventoryApi:HealthCheckEndpoint"];
            var url = $"{apiUrl}{healthEndpoint}";

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (response.IsSuccessStatusCode) {
                   return HealthCheckResult.Healthy("Inventory API is healthy.");
                }
                    return HealthCheckResult.Unhealthy("Inventory API is unhealthy.");
            }
            catch (Exception ex) 
            {
                return HealthCheckResult.Unhealthy("Inventory API is unhealthy", ex);
            }
        }
    }
}
