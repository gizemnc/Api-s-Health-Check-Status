using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Blazor_ApiStatusCheck.HealthChecks
{
    public class HttpHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpointUrl;

        public HttpHealthCheck(HttpClient httpClient, string endpointUrl)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpointUrl = endpointUrl ?? throw new ArgumentNullException(nameof(endpointUrl));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var request = new HttpRequestMessage(HttpMethod.Get, _endpointUrl);

            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken);
                sw.Stop();

                if (response.IsSuccessStatusCode)
                {
                    var duration = sw.ElapsedMilliseconds;
                    return HealthCheckResult.Healthy($"HTTP endpoint is responding with success.Response time : {duration} ms.");
                }
                else
                {
                    var duration = sw.ElapsedMilliseconds;
                    return HealthCheckResult.Unhealthy($"HTTP endpoint responded with status code {response.StatusCode}. Response time: {duration} ms");

                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                var duration = sw.ElapsedMilliseconds;
                return HealthCheckResult.Unhealthy($"Failed to reach HTTP endpoint: {ex.Message}. Response time: {duration} ms");
            }
        }
    }
}
