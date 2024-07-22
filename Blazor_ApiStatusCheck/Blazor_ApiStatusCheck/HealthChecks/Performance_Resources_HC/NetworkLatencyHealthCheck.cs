using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_ApiStatusCheck.HealthChecks.Performance_Resources_HC
{

    public class NetworkLatencyHealthCheck : IHealthCheck
    {
        private readonly string _url;
        private readonly int _maxAllowedLatencyMilliseconds;

        public NetworkLatencyHealthCheck(string url, int maxAllowedLatencyMilliseconds)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _maxAllowedLatencyMilliseconds = maxAllowedLatencyMilliseconds;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var startTime = DateTime.UtcNow;
                    var response = await httpClient.GetAsync(_url, cancellationToken);
                    var endTime = DateTime.UtcNow;

                    var latency = (endTime - startTime).TotalMilliseconds;

                    if (!response.IsSuccessStatusCode)
                    {
                        return HealthCheckResult.Unhealthy($"Request to {_url} failed with status code {response.StatusCode}.");
                    };
                    if (latency > _maxAllowedLatencyMilliseconds)
                    {
                        return HealthCheckResult.Degraded($"Request to {_url} took {latency} ms, which is longer than the allowed {_maxAllowedLatencyMilliseconds} ms.");
                    }
                    return HealthCheckResult.Healthy($"Request to {_url} took {latency} ms.");
                }
                catch (Exception ex) {
                    return HealthCheckResult.Unhealthy($"Exception during network latency check for {_url} : {ex.Message}");
                }
            }
        }
    }
}
