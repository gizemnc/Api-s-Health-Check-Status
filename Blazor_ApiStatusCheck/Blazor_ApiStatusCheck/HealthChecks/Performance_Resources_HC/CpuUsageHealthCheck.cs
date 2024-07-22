using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Blazor_ApiStatusCheck.HealthChecks.Performance_Resources_HC
{
    public class CpuUsageHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            var cpuUsage = GetCpuUsage();
            var formattedCpuUsage = cpuUsage.ToString("F2");


            if (cpuUsage < 80)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"CPU usage is at {formattedCpuUsage}%"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"CPU usage is at {formattedCpuUsage}%"));
            }
        }
        private double GetCpuUsage()
        {
            if (OperatingSystem.IsWindows())
            {
                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
                Thread.Sleep(1000);
                return cpuCounter.NextValue();
            }
            else
                throw new Exception("Only available on Windows operating system!");
        }

    }
}
