using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Blazor_ApiStatusCheck.HealthChecks.Performance_Resources_HC
{

    public class MemoryCheckOptions
    {
        public int Threshold { get; set; }
    }

    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly MemoryCheckOptions _options;

        public MemoryHealthCheck(IOptions<MemoryCheckOptions> options)
        {
            _options = options.Value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var allocated = GC.GetTotalMemory(false);
            var threshold = _options.Threshold;

            if (allocated < threshold)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"Allocated memory is {allocated} bytes, which is below the threshold of {threshold} bytes."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy($"Allocated memory is {allocated} bytes, which is above the threshold of {threshold} bytes."));
        }
    }
}
