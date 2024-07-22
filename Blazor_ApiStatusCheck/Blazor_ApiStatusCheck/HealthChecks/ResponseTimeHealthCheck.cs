using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_ApiStatusCheck.HealthChecks
{
    public class ResponseTimeHealthCheck : IHealthCheck
    {
        private  Random random = new Random();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
           int responseTimeInMS = random.Next(1, 300);
            if(responseTimeInMS < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"The response time looks good ({responseTimeInMS})."));
            } else if(responseTimeInMS < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"The response time is a bit slow ({responseTimeInMS})."));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable ({responseTimeInMS})."));
            }
        }
    }

    public static class HelperMethods
    {
        
    }
}
